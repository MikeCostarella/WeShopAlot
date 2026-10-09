using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WeShopAlot.UI.ClientWPF.Models;

namespace WeShopAlot.UI.ClientWPF.Services
{
    /// <summary>The query-string parameters GET /api/product understands (ProductSpecParams on the server).</summary>
    public record ProductQuery(int PageIndex, int PageSize, int? BrandId, int? TypeId, string? Sort, string? Search);

    public interface IProductApiClient
    {
        Uri BaseAddress { get; }
        Task<Page<Product>> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<NamedItem>> GetBrandsAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<NamedItem>> GetTypesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Every REST call the desktop client makes; the same endpoints the Angular services call.
    /// Nothing here knows about EF Core, Redis or SQL Server: the app only sees URLs and JSON.
    /// </summary>
    public class ApiClient : IProductApiClient
    {
        private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
        private readonly HttpClient http;

        public ApiClient(HttpClient http)
        {
            this.http = http;
        }

        public Uri BaseAddress => http.BaseAddress!;

        /// <summary>
        /// The JWT from login. Once set, every request carries "Authorization: Bearer ..." (what Angular's
        /// jwt.interceptor adds).
        /// </summary>
        public string? Token
        {
            get => http.DefaultRequestHeaders.Authorization?.Parameter;
            set => http.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(value) ? null : new AuthenticationHeaderValue("Bearer", value);
        }

        // ---------------------------------------------------------------- products

        public async Task<Page<Product>> GetProductsAsync(ProductQuery query, CancellationToken cancellationToken = default)
        {
            var parameters = new List<string>
            {
                $"pageIndex={query.PageIndex}",
                $"pageSize={query.PageSize}"
            };
            if (query.BrandId is int brandId) parameters.Add($"brandId={brandId}");
            if (query.TypeId is int typeId) parameters.Add($"typeId={typeId}");
            if (!string.IsNullOrWhiteSpace(query.Sort)) parameters.Add($"sort={Uri.EscapeDataString(query.Sort)}");
            if (!string.IsNullOrWhiteSpace(query.Search)) parameters.Add($"search={Uri.EscapeDataString(query.Search.Trim())}");

            // e.g. GET https://localhost:7244/api/product?pageIndex=1&pageSize=10&brandId=2&sort=priceAsc
            return await GetAsync<Page<Product>>("product?" + string.Join("&", parameters), cancellationToken)
                ?? new Page<Product>(query.PageIndex, query.PageSize, 0, Array.Empty<Product>());
        }

        public async Task<Product?> GetProductAsync(int id) => await GetAsync<Product>($"product/{id}");

        public async Task<IReadOnlyList<NamedItem>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
            await GetAsync<List<NamedItem>>("product/brands", cancellationToken) ?? new List<NamedItem>();

        public async Task<IReadOnlyList<NamedItem>> GetTypesAsync(CancellationToken cancellationToken = default) =>
            await GetAsync<List<NamedItem>>("product/types", cancellationToken) ?? new List<NamedItem>();

        // ---------------------------------------------------------------- basket (no sign-in needed)

        public async Task<CustomerBasket?> GetBasketAsync(string id) =>
            await GetAsync<CustomerBasket>($"basket?id={Uri.EscapeDataString(id)}");

        public async Task<CustomerBasket?> SetBasketAsync(CustomerBasket basket) =>
            await SendAsync<CustomerBasket>(HttpMethod.Post, "basket", basket);

        public Task DeleteBasketAsync(string id) =>
            SendAsync<object>(HttpMethod.Delete, $"basket?id={Uri.EscapeDataString(id)}");

        // ---------------------------------------------------------------- account

        public async Task<User?> LoginAsync(LoginRequest request) =>
            await SendAsync<User>(HttpMethod.Post, "account/login", request);

        public async Task<User?> RegisterAsync(RegisterRequest request) =>
            await SendAsync<User>(HttpMethod.Post, "account/register", request);

        /// <summary>GET /api/account with the saved token: who am I? (204 when the token names nobody.)</summary>
        public async Task<User?> GetCurrentUserAsync() => await GetAsync<User>("account");

        public async Task<bool> EmailExistsAsync(string email) =>
            await GetAsync<bool>($"account/emailexists?email={Uri.EscapeDataString(email)}");

        /// <summary>The saved default address, or null when there is none yet (204).</summary>
        public async Task<Address?> GetAddressAsync() => await GetAsync<Address>("account/address");

        public async Task<Address?> UpdateAddressAsync(Address address) =>
            await SendAsync<Address>(HttpMethod.Put, "account/address", address);

        // ---------------------------------------------------------------- checkout and orders (signed in)

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync() =>
            await GetAsync<List<DeliveryMethod>>("order/deliveryMethods") ?? new List<DeliveryMethod>();

        /// <summary>Creates or updates the Stripe payment intent; the basket comes back with ClientSecret set.</summary>
        public async Task<CustomerBasket?> CreatePaymentIntentAsync(string basketId) =>
            await SendAsync<CustomerBasket>(HttpMethod.Post, $"payment/{Uri.EscapeDataString(basketId)}", new { });

        public async Task<CreatedOrder?> CreateOrderAsync(OrderToCreate order) =>
            await SendAsync<CreatedOrder>(HttpMethod.Post, "order", order);

        public async Task<IReadOnlyList<Order>> GetOrdersAsync() =>
            await GetAsync<List<Order>>("order") ?? new List<Order>();

        public async Task<Order?> GetOrderAsync(int id) => await GetAsync<Order>($"order/{id}");

        // ---------------------------------------------------------------- plumbing

        private Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default) =>
            SendAsync<T>(HttpMethod.Get, url, null, cancellationToken);

        private async Task<T?> SendAsync<T>(HttpMethod method, string url, object? body = null,
            CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(method, url);
            if (body is not null) request.Content = JsonContent.Create(body, body.GetType(), options: Json);

            using var response = await http.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode) throw await ToApiExceptionAsync(response);

            // 204 No Content (or an empty body) means "nothing to return".
            if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
                return default;
            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            return string.IsNullOrWhiteSpace(text) ? default : JsonSerializer.Deserialize<T>(text, Json);
        }

        private static async Task<ApiException> ToApiExceptionAsync(HttpResponseMessage response)
        {
            var status = response.StatusCode;
            var message = status switch
            {
                HttpStatusCode.Unauthorized => "You need to sign in (or your sign-in has expired).",
                HttpStatusCode.NotFound => "Not found.",
                HttpStatusCode.BadRequest => "The request was not valid.",
                _ => $"The API answered {(int)status} {response.ReasonPhrase}."
            };
            List<string>? errors = null;
            try
            {
                var text = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    using var json = JsonDocument.Parse(text);
                    var root = json.RootElement;
                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String)
                            message = m.GetString() ?? message;
                        if (root.TryGetProperty("errors", out var e))
                        {
                            errors = new List<string>();
                            if (e.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in e.EnumerateArray())
                                    if (item.ValueKind == JsonValueKind.String) errors.Add(item.GetString()!);
                            }
                            else if (e.ValueKind == JsonValueKind.Object) // ASP.NET's ProblemDetails shape
                            {
                                foreach (var field in e.EnumerateObject())
                                    if (field.Value.ValueKind == JsonValueKind.Array)
                                        foreach (var item in field.Value.EnumerateArray())
                                            if (item.ValueKind == JsonValueKind.String) errors.Add(item.GetString()!);
                            }
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // Not JSON; keep the generic message.
            }
            return new ApiException(message, status, errors);
        }
    }
}

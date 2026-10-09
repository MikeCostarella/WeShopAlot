using System.Text.Json.Serialization;

namespace WeShopAlot.UI.ClientWPF.Models
{
    // The shapes the WeShopAlot REST API sends and receives. The API uses camelCase JSON; System.Net.Http.Json
    // matches names without regard to case, so these PascalCase types line up with the Angular models
    // (Client/src/app/shared/models). The desktop client keeps its own copies instead of referencing the
    // server projects, as any outside client of a REST API would.

    // ------------------------------------------------------------------ catalog

    /// <summary>One product from GET /api/product (ProductToReturnDto on the server).</summary>
    public record Product(int Id, string Name, string? Description, decimal Price,
        string? PictureUrl, string ProductType, string ProductBrand);

    /// <summary>A brand or type from GET /api/product/brands or /api/product/types.</summary>
    public record NamedItem(int Id, string Name);

    /// <summary>One page of results (Pagination&lt;T&gt; on the server).</summary>
    public record Page<T>(int PageIndex, int PageSize, int Count, IReadOnlyList<T>? Data);

    /// <summary>A choice in the Sort box; Value is what the API expects (null = by name).</summary>
    public record SortOption(string Label, string? Value);

    // ------------------------------------------------------------------ basket

    /// <summary>A line in the basket (BasketItemDto on the server).</summary>
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; } = "";
        public string Brand { get; set; } = "";
        public string Type { get; set; } = "";

        [JsonIgnore]
        public decimal LineTotal => Price * Quantity;
    }

    /// <summary>The basket kept in Redis by the API (CustomerBasketDto on the server).</summary>
    public class CustomerBasket
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<BasketItem> Items { get; set; } = new();
        public int? DeliveryMethodId { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }

    public record BasketTotals(decimal Shipping, decimal Subtotal, decimal Total);

    // ------------------------------------------------------------------ account

    /// <summary>The signed-in user (UserDto on the server). Token is the JWT sent as "Authorization: Bearer ...".</summary>
    public record User(string Email, string DisplayName, string Token);

    public record LoginRequest(string Email, string Password);

    public record RegisterRequest(string DisplayName, string Email, string Password);

    /// <summary>A shipping address (AddressDto on the server).</summary>
    public class Address
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zipcode { get; set; } = "";
    }

    // ------------------------------------------------------------------ orders

    /// <summary>A shipping option from GET /api/order/deliveryMethods.</summary>
    public record DeliveryMethod(int Id, string ShortName, string DeliveryTime, string Description, decimal Price);

    /// <summary>The body of POST /api/order.</summary>
    public record OrderToCreate(string BasketId, int DeliveryMethodId, Address ShipToAddress);

    /// <summary>POST /api/order answers with the saved order; the client only needs its number.</summary>
    public record CreatedOrder(int Id);

    /// <summary>An order from GET /api/order or /api/order/{id} (OrderToReturnDto on the server).</summary>
    public record Order(int Id, string BuyerEmail, DateTime OrderDate, Address? ShipToAddress, string? DeliveryMethod,
        decimal ShippingPrice, IReadOnlyList<OrderItem>? OrderItems, decimal Subtotal, decimal Total, string? Status)
    {
        /// <summary>The API sends UTC (ending in "Z"); show the user's local time.</summary>
        public DateTime LocalOrderDate => OrderDate.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(OrderDate, DateTimeKind.Utc).ToLocalTime()
            : OrderDate.ToLocalTime();
    }

    public record OrderItem(int ProductId, string ProductName, string? PictureUrl, decimal Price, int Quantity)
    {
        public decimal LineTotal => Price * Quantity;
    }
}

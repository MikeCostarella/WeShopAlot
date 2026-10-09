using System.Collections.ObjectModel;
using System.Net.Http;
using WeShopAlot.UI.ClientWPF.Command;
using WeShopAlot.UI.ClientWPF.Services;
using WeShopAlot.UI.ClientWPF.Models;

namespace WeShopAlot.UI.ClientWPF.ViewModel
{
    /// <summary>
    /// The shop page (Angular's ShopComponent): the catalog from the WeShopAlot REST API. Filters and paging
    /// become query-string parameters on GET /api/product, exactly as in the Angular shop page.
    /// </summary>
    public class ProductsViewModel : ViewModelBase
    {
        private const int PageSize = 10;
        private const int StartupAttempts = 15;
        private static readonly TimeSpan StartupRetryDelay = TimeSpan.FromSeconds(2);
        private static readonly NamedItem All = new(0, "All");

        private readonly IProductApiClient api;
        private readonly BasketService basket;
        private readonly IShell shell;
        private bool lookupsLoaded;
        private bool suppressReload;
        private int requestVersion;

        private NamedItem selectedBrand = All;
        private NamedItem selectedType = All;
        private SortOption selectedSort;
        private string search = "";
        private int pageIndex = 1;
        private int totalCount;
        private bool isLoading;
        private string status = "";

        public ProductsViewModel(IProductApiClient api, BasketService basket, IShell shell)
        {
            this.api = api;
            this.basket = basket;
            this.shell = shell;
            SortOptions = new[]
            {
                new SortOption("Name", null),
                new SortOption("Price: low to high", "priceAsc"),
                new SortOption("Price: high to low", "priceDesc")
            };
            selectedSort = SortOptions[0];

            SearchCommand = new DelegateCommand(async _ => await ReloadFromFirstPageAsync());
            ResetCommand = new DelegateCommand(async _ => await ResetAsync());
            RefreshCommand = new DelegateCommand(async _ => await LoadAsync());
            PreviousPageCommand = new DelegateCommand(async _ => await GoToPageAsync(pageIndex - 1), _ => !IsLoading && pageIndex > 1);
            NextPageCommand = new DelegateCommand(async _ => await GoToPageAsync(pageIndex + 1), _ => !IsLoading && pageIndex < PageCount);
            AddToBasketCommand = new DelegateCommand(async p => await AddToBasketAsync(p as Product));
            DetailsCommand = new DelegateCommand(p => { if (p is Product product) shell.GoProduct(product.Id); });
        }

        public ObservableCollection<Product> Products { get; } = new();
        public ObservableCollection<NamedItem> Brands { get; } = new() { All };
        public ObservableCollection<NamedItem> Types { get; } = new() { All };
        public IReadOnlyList<SortOption> SortOptions { get; }

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand ResetCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand PreviousPageCommand { get; }
        public DelegateCommand NextPageCommand { get; }
        public DelegateCommand AddToBasketCommand { get; }
        public DelegateCommand DetailsCommand { get; }

        public NamedItem SelectedBrand
        {
            get => selectedBrand;
            set { if (SetProperty(ref selectedBrand, value ?? All)) _ = ReloadFromFirstPageAsync(); }
        }

        public NamedItem SelectedType
        {
            get => selectedType;
            set { if (SetProperty(ref selectedType, value ?? All)) _ = ReloadFromFirstPageAsync(); }
        }

        public SortOption SelectedSort
        {
            get => selectedSort;
            set { if (SetProperty(ref selectedSort, value ?? SortOptions[0])) _ = ReloadFromFirstPageAsync(); }
        }

        /// <summary>Typed text; applied when the user presses Enter or clicks Search.</summary>
        public string Search
        {
            get => search;
            set => SetProperty(ref search, value ?? "");
        }

        public int TotalCount
        {
            get => totalCount;
            private set
            {
                if (SetProperty(ref totalCount, value)) RaisePropertyChanged(nameof(PageSummary));
            }
        }

        public int PageCount => Math.Max(1, (int)Math.Ceiling(totalCount / (double)PageSize));

        public string PageSummary => totalCount == 0
            ? "No products"
            : $"Page {pageIndex} of {PageCount}  ·  {totalCount} products";

        public bool IsLoading
        {
            get => isLoading;
            private set
            {
                if (SetProperty(ref isLoading, value)) RaiseCommandStates();
            }
        }

        /// <summary>One line at the bottom of the view: which URL was called, or what went wrong.</summary>
        public string Status
        {
            get => status;
            private set => SetProperty(ref status, value);
        }

        public override async Task LoadAsync()
        {
            if (!lookupsLoaded && !await LoadLookupsAsync()) return;
            await LoadPageAsync();
        }

        /// <summary>
        /// Brands and types fill the filter boxes. When Visual Studio starts the API and this window together,
        /// the API may still be starting, so keep trying for about 30 seconds before giving up.
        /// </summary>
        private async Task<bool> LoadLookupsAsync()
        {
            for (var attempt = 1; attempt <= StartupAttempts; attempt++)
            {
                try
                {
                    var brands = await api.GetBrandsAsync();
                    var types = await api.GetTypesAsync();

                    suppressReload = true;
                    Replace(Brands, brands);
                    Replace(Types, types);
                    SelectedBrand = All;
                    SelectedType = All;
                    suppressReload = false;

                    lookupsLoaded = true;
                    return true;
                }
                catch (Exception ex) when (IsConnectionProblem(ex))
                {
                    Status = $"Waiting for the API at {api.BaseAddress} (attempt {attempt} of {StartupAttempts})…";
                    if (attempt < StartupAttempts) await Task.Delay(StartupRetryDelay);
                }
            }

            Status = $"Can't reach the API at {api.BaseAddress}. Start WeShopAlot.WebAPI (https profile), then click Refresh.";
            return false;
        }

        private async Task LoadPageAsync()
        {
            var version = ++requestVersion;
            IsLoading = true;
            var query = new ProductQuery(
                pageIndex,
                PageSize,
                selectedBrand.Id == 0 ? null : selectedBrand.Id,
                selectedType.Id == 0 ? null : selectedType.Id,
                selectedSort.Value,
                search);

            try
            {
                var page = await api.GetProductsAsync(query);
                if (version != requestVersion) return; // a newer request has started; ignore this answer

                Replace(Products, page.Data ?? Array.Empty<Product>());
                TotalCount = page.Count;
                RaisePropertyChanged(nameof(PageSummary));
                Status = $"GET {api.BaseAddress}product?pageIndex={pageIndex}&pageSize={PageSize}"
                    + (query.BrandId is int b ? $"&brandId={b}" : "")
                    + (query.TypeId is int t ? $"&typeId={t}" : "")
                    + (query.Sort is string s ? $"&sort={s}" : "")
                    + (string.IsNullOrWhiteSpace(query.Search) ? "" : $"&search={query.Search.Trim()}");
            }
            catch (Exception ex) when (IsConnectionProblem(ex))
            {
                if (version == requestVersion) Status = $"Couldn't load products: {ex.Message} Click Refresh to try again.";
            }
            finally
            {
                if (version == requestVersion) IsLoading = false;
            }
        }

        private async Task AddToBasketAsync(Product? product)
        {
            if (product is null) return;
            try
            {
                await basket.AddItemAsync(product);
                shell.ShowMessage($"Added {product.Name} to your basket.");
            }
            catch (Exception ex) when (IsConnectionProblem(ex))
            {
                shell.ShowMessage(ex is ApiException apiError ? apiError.Details : ex.Message, isError: true);
            }
        }

        private async Task ReloadFromFirstPageAsync()
        {
            if (suppressReload || !lookupsLoaded) return;
            pageIndex = 1;
            await LoadPageAsync();
        }

        private async Task GoToPageAsync(int newPageIndex)
        {
            pageIndex = Math.Clamp(newPageIndex, 1, PageCount);
            await LoadPageAsync();
        }

        private async Task ResetAsync()
        {
            suppressReload = true;
            SelectedBrand = All;
            SelectedType = All;
            SelectedSort = SortOptions[0];
            Search = "";
            suppressReload = false;
            await ReloadFromFirstPageAsync();
        }

        private void RaiseCommandStates()
        {
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
        }

        private static bool IsConnectionProblem(Exception ex) =>
            ex is HttpRequestException or TaskCanceledException or System.Text.Json.JsonException;

        private static void Replace<T>(ObservableCollection<T> target, IEnumerable<T> items)
        {
            var keep = target.Count > 0 && Equals(target[0], All) ? 1 : 0; // Brands and Types keep "All" first
            while (target.Count > keep) target.RemoveAt(target.Count - 1);
            foreach (var item in items) target.Add(item);
        }
    }
}

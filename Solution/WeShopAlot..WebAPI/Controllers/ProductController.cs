using AutoMapper;
using WeShopAlot.Data.Models;
using WeShopAlot.Data.Repositories.Interfaces;
using WeShopAlot.Data.Specifications;
using WeShopAlot.Shared.Dtos;
using WeShopAlot.WebAPI.Errors;
using WeShopAlot.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WeShopAlot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        #region Member Variables

        private readonly IProductBrandRepository productBrandRepository;
        private readonly IProductTypeRepository productTypeRepository;
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        #endregion Member Variables

        #region Constructors

        public ProductController(IProductRepository productRepository,
            IProductTypeRepository productTypeRepoitory,
            IProductBrandRepository productBrandRepoitory
            , IMapper mapper)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.productTypeRepository = productTypeRepoitory;
            this.productBrandRepository = productBrandRepoitory;
        }

        #endregion Constructors

        #region Actions

        //[Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);
            var totalItems = await productRepository.CountAsync(countSpec);
            var products = await productRepository.ListAsync(spec);
            try
            {
                var data = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
                return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await productRepository.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        //[Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productBrandRepository.ListAllAsync());
        }

        //[Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await productTypeRepository.ListAllAsync());
        }

        #endregion Actions
    }
}

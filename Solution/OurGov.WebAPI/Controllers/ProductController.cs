using AutoMapper;
using MeShopAlot.Data.Models;
using MeShopAlot.Data.Repositories.Interfaces;
using MeShopAlot.Data.Specifications;
using MeShopAlot.WebAPI.Dtos;
using MeShopAlot.WebAPI.Errors;
using MeShopAlot.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MeShopAlot.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductBrandRepository productBrandRepository;
        private readonly IProductTypeRepository productTypeRepository;
        private readonly IProductRepository productsRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productsRepository,
            IProductTypeRepository productTypeRepoitory,
            IProductBrandRepository productBrandRepoitory
            , IMapper mapper)
        {
            this.mapper = mapper;
            this.productsRepository = productsRepository;
            this.productTypeRepository = productTypeRepoitory;
            this.productBrandRepository = productBrandRepoitory;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await productsRepository.CountAsync(countSpec);
            var products = await productsRepository.ListAsync(spec);

            var data = mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await productsRepository.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productBrandRepository.ListAllAsync());
        }

        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await productTypeRepository.ListAllAsync());
        }
    }
}

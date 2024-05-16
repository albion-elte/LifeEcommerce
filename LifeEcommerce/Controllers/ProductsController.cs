using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Services;
using LifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace LifeEcommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get(string searchText, int page = 1, int pageSize = 10, int categoryId = 0)
        {
            var products = await _productService.ProductsListView(searchText, page, pageSize, categoryId);

            return Ok(products);
        }

        [HttpPost(Name = "CreateProducts")]
        public async Task<IActionResult> Create(ProductCreateDto productToCreate)
        {
            await _productService.CreateProduct(productToCreate);

            return Ok();
        }
    }
}

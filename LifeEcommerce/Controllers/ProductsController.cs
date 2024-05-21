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

        [HttpGet(Name = "ProductsListView")]
        public async Task<IActionResult> ProductsListView(string? searchText, int page = 1, int pageSize = 10, int categoryId = 0)
        {
            var products = await _productService.ProductsListView(searchText, page, pageSize, categoryId);

            return Ok(products);
        }

        [HttpGet("all",Name = "GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _productService.GetProduct(int.Parse(id));

            return Ok(product);
        }

        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Create(ProductCreateDto productToCreate)
        {
            await _productService.CreateProduct(productToCreate);

            return Ok();
        }

        [HttpPut(Name = "UpdateProduct")]
        public async Task<IActionResult> Update(ProductDto productToUpdate)
        {
            await _productService.UpdateProduct(productToUpdate);

            return Ok();
        }

        [HttpDelete(Name = "DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);

            return Ok();
        }
    }
}

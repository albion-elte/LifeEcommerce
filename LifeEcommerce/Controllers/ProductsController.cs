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
        private readonly ImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService, ImageUploadService imageUploadService, IConfiguration configuration)
        {
            _logger = logger;
            _productService = productService;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
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

            //try
            //{
            //    int a = 1;
            //    int b = 0;

            //    int c = a / b;
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex, "Error Life");
            //    _logger.LogInformation(ex, "Information Life");
            //    _logger.LogDebug(ex, "Debug Life");
            //}

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

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var uploadedImage = await _imageUploadService.UploadPicture(file, _configuration);

            return Ok(uploadedImage);
        }
    }
}

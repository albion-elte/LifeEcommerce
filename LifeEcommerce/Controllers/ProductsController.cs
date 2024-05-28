using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Services;
using LifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LifeEcommerce.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<ProductsController> _stringLocalizer;


        public ProductsController(ILogger<ProductsController> logger, IProductService productService, ImageUploadService imageUploadService, IConfiguration configuration, IStringLocalizer<ProductsController> stringLocalizer)
        {
            _logger = logger;
            _productService = productService;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
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

            //var message = _stringLocalizer["ProductCreatedMessage"];
            var message = _stringLocalizer.GetString("ProductCreatedMessage").Value;

            //return Ok(message.Value);
            return Ok(message);
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

        [NonAction]
        public static List<string> ListOfCountries()
        {
            List<string> countryList = new List<string>();
            CultureInfo[] countryInfoList = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo countryInfo in countryInfoList)
            {
                try
                {
                    RegionInfo R = new RegionInfo(countryInfo.LCID);
                    if (!(countryList.Contains(R.EnglishName)))
                    {
                        countryList.Add(R.EnglishName);
                    }
                }
                catch
                {
                    continue;
                }
            }

            countryList.Add("Kosovo");

            countryList.Sort();

            return countryList;
        }
    }
}

using LifeEcommerce.Services;
using LifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeEcommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly ImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, ImageUploadService imageUploadService, IConfiguration configuration, IEmailSender emailSender)
        {
            _logger = logger;
            _categoryService = categoryService;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpGet(Name = "CategorysListView")]
        public async Task<IActionResult> CategorysListView(string? searchText, int page = 1, int pageSize = 10)
        {
            var categories = await _categoryService.CategoriesListView(searchText, page, pageSize);   

            return Ok(categories);
        }
    }
}

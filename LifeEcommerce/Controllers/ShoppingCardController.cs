using LifeEcommerce.Services.IService;
using LifeEcommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using LifeEcommerce.Models.Dtos.Product;
using System.Security.Claims;

namespace LifeEcommerce.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ShoppingCardController : ControllerBase
    {
        private readonly ILogger<ShoppingCardController> _logger;
        private readonly IShoppingCardService _shoppingCardService;
        private readonly ImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<ShoppingCardController> _stringLocalizer;


        public ShoppingCardController(ILogger<ShoppingCardController> logger, IShoppingCardService shoppingCardService, ImageUploadService imageUploadService, IConfiguration configuration, IStringLocalizer<ShoppingCardController> stringLocalizer)
        {
            _logger = logger;
            _shoppingCardService = shoppingCardService;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet(Name = "ShoppingCardListView")]
        public async Task<IActionResult> ShoppingCardListView(string? searchText, int page = 1, int pageSize = 10, int categoryId = 0)
        {
            var shoppingCards = await _shoppingCardService.ShoppingCardListView(searchText, page, pageSize, categoryId);

            return Ok(shoppingCards);
        }

        [HttpGet("all", Name = "GetShoppingCard")]
        public async Task<IActionResult> GetShoppingCard()
        {
            var shoppingCards = await _shoppingCardService.GetAllShoppingCards();

            return Ok(shoppingCards);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var shoppingCard = await _shoppingCardService.GetShoppingCard(int.Parse(id));

            return Ok(shoppingCard);
        }

        [HttpGet("ViewUsersShoppingCardItems")]
        public async Task<IActionResult> ViewUsersShoppingCardItems()
        {
            var userId = User.Identities.First().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var shoppingCard = await _shoppingCardService.GetUsersShoppingCardItems(userId);

            return Ok(shoppingCard);
        }

        [HttpPost(Name = "AddToCard")]
        public async Task<IActionResult> Create(ShoppingCardCreateDto shoppingCardToCreate)
        {
            await _shoppingCardService.CreateShoppingCard(shoppingCardToCreate);

            return Ok();
        }

        [HttpPut("IncreaseItemCount")]
        public async Task<IActionResult> IncreaseItemCount(int shoppingCardItemId)
        {
            await _shoppingCardService.IncreaseItemCount(shoppingCardItemId);

            return Ok();
        }

        [HttpPut("DecreaseItemCount")]
        public async Task<IActionResult> DecreaseItemCount(int shoppingCardItemId)
        {
            await _shoppingCardService.DecreaseItemCount(shoppingCardItemId);

            return Ok();
        }

        [HttpPut("UpdateShoppingCard")]
        public async Task<IActionResult> Update(ShoppingCardDto shoppingCardToUpdate)
        {
            await _shoppingCardService.UpdateShoppingCard(shoppingCardToUpdate);

            return Ok();
        }

        [HttpDelete(Name = "DeleteShoppingCard")]
        public async Task<IActionResult> Delete(int id)
        {
            await _shoppingCardService.DeleteShoppingCard(id);

            return Ok();
        }

        [HttpDelete("EmptyShoppingCard")]
        public async Task<IActionResult> EmptyShoppingCard()
        {
            var userId = User.Identities.First().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            await _shoppingCardService.EmptyShoppingCard(userId);

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

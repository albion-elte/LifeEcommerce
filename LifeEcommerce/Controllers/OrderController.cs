using LifeEcommerce.Services.IService;
using LifeEcommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using LifeEcommerce.Models.Dtos.Product;
using System.Security.Claims;
using LifeEcommerce.Models.Dtos.Order;

namespace LifeEcommerce.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<OrderController> _stringLocalizer;


        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ImageUploadService imageUploadService, IConfiguration configuration, IStringLocalizer<OrderController> stringLocalizer)
        {
            _logger = logger;
            _orderService = orderService;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            await _orderService.CreateOrder(orderCreateDto);

            return Ok();
        }

        [HttpPost("OrderSummary")]
        public async Task<IActionResult> OrderSummary(/*Create model as front already has userdata*/)
        {
            await _orderService.OrderSummary(User);

            return Ok();
        }
    }
}

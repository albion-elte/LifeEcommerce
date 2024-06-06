using AutoMapper;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Order;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LifeEcommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateOrder(OrderCreateDto orderCreateDto)
        {
            var orderData = _mapper.Map<OrderData>(orderCreateDto);

            orderData.Id = Guid.NewGuid().ToString();

            _unitOfWork.Repository<OrderData>().Create(orderData);

            var productPrice = await _unitOfWork.Repository<Product>().GetById(x => x.Id == orderCreateDto.ProductId).Select(x => x.Price).FirstOrDefaultAsync();

            var orderDetails = new OrderDetail
            {
                OrderId = orderData.Id,
                ProductId = orderCreateDto.ProductId,
                UserId = orderCreateDto.UserId,
                Count = orderCreateDto.Count,
                Price = productPrice * orderCreateDto.Count
            };

            _unitOfWork.Repository<OrderDetail>().Create(orderDetails);

            var shoppingCardItemToDelete = await _unitOfWork.Repository<ShoppingCard>().GetByCondition(x => x.ProductId == orderCreateDto.ProductId && x.UserId == orderCreateDto.UserId).FirstOrDefaultAsync();

            _unitOfWork.Repository<ShoppingCard>().Delete(shoppingCardItemToDelete);

            _unitOfWork.Complete();
        }

        public async Task OrderSummary(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = user.FindFirst(ClaimTypes.Surname)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var gender = user.FindFirst(ClaimTypes.Gender)?.Value;
            var birthdate = user.FindFirst(ClaimTypes.DateOfBirth)?.Value;
            var address = user.FindFirst("address")?.Value;

            var street = user.FindFirst("street")?.Value ?? "street";
            var city = user.FindFirst("locality")?.Value ?? "city";
            var countrry = user.FindFirst("region")?.Value ?? "country";
            short postalCode = 10;

            var shoppingCardItems = await _unitOfWork.Repository<ShoppingCard>().GetByCondition(x => x.UserId == userId)
                                                .Include(x => x.Product)
                                                .ToListAsync();

            //var productIds = shoppingCardItems.Select(x => x.ProductId).ToList();

            //var productPricesDictionary = await _unitOfWork.Repository<Product>().GetByCondition(x => productIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, y => y.Price);

            var orderDataList = new List<OrderData>();
            var orderDetailsList = new List<OrderDetail>();

            foreach (var item in shoppingCardItems)
            {
                var orderData = new OrderData
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    ShippingDate = DateTime.Now.AddDays(7),
                    PaymentDate = DateTime.Now,
                    OrderTotal = item.Count * item.Product.Price,
                    OrderStatus = ProjectConstants.Created,
                    PhoneNumber = "123",
                    StreetAddress = street,
                    City = city,
                    Country = countrry,
                    PostalCode = postalCode,
                    Name = firstName
                };

                orderDataList.Add(orderData);

                var orderDetails = new OrderDetail
                {
                    OrderId = orderData.Id,
                    ProductId = item.ProductId,
                    UserId = userId,
                    Count = item.Count,
                    Price = item.Count * item.Product.Price
                };

                orderDetailsList.Add(orderDetails);
            }

            _unitOfWork.Repository<OrderData>().CreateRange(orderDataList);
            _unitOfWork.Repository<OrderDetail>().CreateRange(orderDetailsList);

            _unitOfWork.Repository<ShoppingCard>().DeleteRange(shoppingCardItems);

            _unitOfWork.Complete();
        }
    }
}

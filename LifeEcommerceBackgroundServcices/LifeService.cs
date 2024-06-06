using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerceBackgroundServcices
{
    public class LifeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LifeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateOrderStatus()
        {
            var orders = await _unitOfWork.Repository<OrderData>().GetByCondition(x => x.OrderStatus == "Created" && x.OrderDate < DateTime.Now.AddHours(-1)).ToListAsync();

            orders.ForEach(x => x.OrderStatus = "Processing");

            _unitOfWork.Complete();
        }
    }
}

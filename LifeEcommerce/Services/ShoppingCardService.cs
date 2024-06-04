using AutoMapper;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Product;
using LifeEcommerce.Models.Dtos.ShoppingCard;
using LifeEcommerce.Models.Entities;
using LifeEcommerce.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Services
{
    public class ShoppingCardService : IShoppingCardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCardService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateShoppingCard(ShoppingCardCreateDto shoppingCard)
        {
            var existingShoppingCardItem = await _unitOfWork.Repository<ShoppingCard>()
                .GetByCondition(x => x.UserId == shoppingCard.UserId && x.ProductId == shoppingCard.ProductId).FirstOrDefaultAsync();

            if (existingShoppingCardItem != null)
            {
                existingShoppingCardItem.Count += shoppingCard.Count;
            }
            else
            {
                var shoppingCardToAdd = _mapper.Map<ShoppingCard>(shoppingCard);

                _unitOfWork.Repository<ShoppingCard>().Create(shoppingCardToAdd);
            }

            _unitOfWork.Complete();
        }

        public Task DeleteShoppingCard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShoppingCard>> GetAllShoppingCards()
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCard> GetShoppingCard(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCardItemsDto> GetUsersShoppingCardItems(string? userId)
        {
            var currentUserItems = await _unitOfWork.Repository<ShoppingCard>().GetByCondition(x => x.UserId == userId)
                                                                  .Include(x => x.Product)
                                                                  .Select(x => new ShoppingCardDto
                                                                  {   
                                                                     Id = x.Id,
                                                                     UserId = x.UserId,
                                                                     Count = x.Count, 
                                                                     ProductId = x.Product.Id, 
                                                                     ProductName = x.Product.Name, 
                                                                     ProductImage = x.Product.ImageUrl, 
                                                                     ProductPrice = x.Product.Price,
                                                                     //ItemTotal = x.Count * x.Price
                                                                  })
                                                                  .ToListAsync();

            currentUserItems.ForEach(x => x.ItemTotal = x.ProductPrice * x.Count);


            var shoppingCardItems = new ShoppingCardItemsDto
            {
                ShoppingCardItems = currentUserItems,
                //TotalOnCard = currentUserItems.Select(x => x.ItemTotal).Sum()
                TotalOnCard = currentUserItems.Sum(x => x.ItemTotal)
            };

            return shoppingCardItems;
        }

        public async Task IncreaseItemCount(int shoppingCardItemId)
        {
            var shoppingCardItem = await _unitOfWork.Repository<ShoppingCard>().GetById(x => x.Id == shoppingCardItemId).FirstOrDefaultAsync();

            shoppingCardItem.Count++;

            _unitOfWork.Complete();
        }

        public async Task DecreaseItemCount(int shoppingCardItemId)
        {
            var shoppingCardItem = await _unitOfWork.Repository<ShoppingCard>().GetById(x => x.Id == shoppingCardItemId).FirstOrDefaultAsync();

            shoppingCardItem.Count--;

            _unitOfWork.Complete();
        }

        public Task<PagedInfo<ShoppingCardDto>> ShoppingCardListView(string search, int page, int pageSize, int categoryId = 0)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShoppingCard(ShoppingCardDto shoppingCard)
        {
            throw new NotImplementedException();
        }

        public async Task EmptyShoppingCard(string? userId)
        {
            var shoppingCardItems = await _unitOfWork.Repository<ShoppingCard>().GetByCondition(x => x.UserId == userId)
                                                                   .ToListAsync();

            _unitOfWork.Repository<ShoppingCard>().DeleteRange(shoppingCardItems);

            _unitOfWork.Complete();
        }
    }
}

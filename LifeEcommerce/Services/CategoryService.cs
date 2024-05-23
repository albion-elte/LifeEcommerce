using AutoMapper;
using LifeEcommerce.Data.UnitOfWork;
using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Category;
using LifeEcommerce.Services.IService;
using System.Linq.Expressions;
using LifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeEcommerce.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedInfo<CategoryDto>> CategoriesListView(string search, int page = 1, int pageSize = 10)
        {
            //Expression<Func<Category, bool>> condition = x => x.Name.Contains(search);

            var categories = _unitOfWork.Repository<Category>()
                                                                 .GetAll()
                                                                 .WhereIf(!string.IsNullOrEmpty(search), x => x.Name.Contains(search));

            var count = await categories.CountAsync();

            var items = _mapper.Map<List<CategoryDto>>(await categories.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());


            var categoriesPaged = new PagedInfo<CategoryDto>()
            {
                TotalCount = count,
                Page = page,
                PageSize = pageSize,
                Items = items
            };

            return categoriesPaged;
        }

        public Task CreateCategory(CategoryCreateDto categoryToCreate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategory(CategoryDto categoryToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}

using LifeEcommerce.Helpers;
using LifeEcommerce.Models.Dtos.Category;

namespace LifeEcommerce.Services.IService
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryCreateDto categoryToCreate);
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategory(int id);
        Task<PagedInfo<CategoryDto>> CategoriesListView(string search, int page = 1, int pageSize = 10);
        Task UpdateCategory(CategoryDto categoryToUpdate);
        Task DeleteCategory(int id);
    }
}

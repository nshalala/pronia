using Pronia.Models;
using Pronia.ViewModels.CategoryVMs;

namespace Pronia.Services.Interfaces;

public interface ICategoryService
{
    Task Create(CreateCategoryVM vm);
    Task Update(UpdateCategoryVM vm);
    Task Delete(int? id);
    Task<ICollection<Category>> GetAll();
    Task<Category> GetById(int? id);
}

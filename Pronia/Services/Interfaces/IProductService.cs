using Pronia.Models;
using Pronia.ViewModels.ProductVMs;

namespace Pronia.Services.Interfaces;

public interface IProductService
{
    public IQueryable<Product> GetTable { get; }
    public Task<List<Product>> GetAllAsync(bool takeAll);
    public Task<Product> GetByIdAsync(int? id);
    public Task Create(CreateProductVM productVm);
    public Task Update(int? id, UpdateProductVM productVm);
    public Task Delete(int id);
    public Task SoftDelete(int id);
    public Task DeleteImage(int? id);
}

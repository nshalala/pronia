using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Helpers.Exceptions;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.CategoryVMs;

namespace Pronia.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ProniaDbContext _context;

    public CategoryService(ProniaDbContext context)
    {
        _context = context;
    }

    public async Task Create(CreateCategoryVM vm)
    {
        if(await _context.Categories.AnyAsync(c=>c.Name == vm.Name)) throw new AlreadyExistsException("This category already exists");
        Category category = new Category() { Name = vm.Name };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Category>> GetAll() => await _context.Categories.ToListAsync();

    public Task<Category> GetById(int? id)
    {
        throw new NotImplementedException();
    }

    public Task Update(UpdateCategoryVM vm)
    {
        throw new NotImplementedException();
    }
}

using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Helpers.Exceptions;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.ProductVMs;

namespace Pronia.Services.Implementations;

public class ProductService : IProductService
{
    private readonly ProniaDbContext _context;
    private readonly IFileService _fileService;

    public IQueryable<Product> GetTable { get => _context.Set<Product>(); }

    public ProductService(ProniaDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task Create(CreateProductVM productVm)
    {
        Product entity = new Product()
        {
            Name = productVm.Name,
            Description = productVm.Description,
            Discount = productVm.Discount,
            Rating = productVm.Rating,
            Price = productVm.Price,
            StockCount = productVm.StockCount,
            MainImage = await _fileService.UploadAsync(productVm.MainImageFile, Path.Combine("assets", "imgs", "products"))
        };
        if (productVm.HoverImageFile != null)
            entity.HoverImage = await _fileService.UploadAsync(productVm.HoverImageFile, Path.Combine("assets", "imgs", "products"));
        if(productVm.ImageFiles != null)
        {
            List<ProductImage> imgs = new();
            foreach (var item in productVm.ImageFiles)
            {
                var fileName = await _fileService.UploadAsync(item, Path.Combine("assets", "imgs", "products"));
                imgs.Add(new ProductImage
                {
                    Name = fileName
                });
            }
            entity.ProductImages = imgs;
        }
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id);
        if (entity == null) throw new NotFoundException("product not found");
        _fileService.Delete(entity.MainImage);
        if(entity.HoverImage != null) _fileService.Delete(entity.HoverImage);
        if(entity.ProductImages != null)
        {
            foreach (var prodImg in entity.ProductImages) 
            {
                _fileService.Delete(prodImg.Name);
            }
        }
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task SoftDelete(int id)
    {
        var entity = await GetByIdAsync(id);
        entity.IsDeleted = !entity.IsDeleted;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllAsync(bool takeAll)
    {
        if (takeAll)
        {
            return await _context.Products.ToListAsync();
        }
        return await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int? id)
    {
        if (id <= 0 || id == null) throw new ArgumentNullOrNegativeException("Id is not valid");
        var entity = await _context.Products.FindAsync(id);
        if (entity is null) throw new NotFoundException("Product not found");
        return entity;
    }

    public async Task Update(int? id, UpdateProductVM vm)
    {
        var entity = await GetByIdAsync(id);
        entity.Name = vm.Name;
        entity.Price = vm.Price;
        entity.Rating = vm.Rating;
        entity.Description = vm.Description;
        entity.Discount = vm.Discount;
        if(vm.MainImageFile != null)
        {
            _fileService.Delete(entity.MainImage);
            entity.MainImage = await _fileService.UploadAsync(vm.MainImageFile, Path.Combine("assets", "imgs", "products"));
        }
        if(vm.HoverImageFile != null)
        {
            if(entity.HoverImage != null) _fileService.Delete(entity.HoverImage);
            entity.HoverImage = await _fileService.UploadAsync(vm.HoverImageFile, Path.Combine("assets", "imgs", "products"));
        }
        if(vm.ProductImageFiles != null)
        {
            if (entity.ProductImages == null) entity.ProductImages = new List<ProductImage>();
            foreach (var imgs in vm.ProductImageFiles)
            {
                ProductImage img = new ProductImage
                {
                    Name = await _fileService.UploadAsync(imgs, Path.Combine("assets", "imgs", "products"))
                };
                entity.ProductImages.Add(img);
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteImage(int? id)
    {
        if (id == null || id <= 0) throw new ArgumentNullOrNegativeException("id is not valid");
        var prodImg = await _context.ProductImages.FindAsync(id);
        if (prodImg == null) throw new NotFoundException("Image not found");
        _fileService.Delete(prodImg.Name);
        _context.ProductImages.Remove(prodImg);
        await _context.SaveChangesAsync();
    }
}

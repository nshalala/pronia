using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Helpers.Exceptions;
using Pronia.Helpers.Extensions;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.ProductVMs;

namespace Pronia.Areas.Manage.Controllers;

[Area("Manage")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAllAsync(true));
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVM vm)
    {
        if(vm.MainImageFile != null)
        {
            if (!vm.MainImageFile.isValidSize(2)) throw new SizeLimitException("Image size cannot be greater than 2 mb");
            if (!vm.MainImageFile.isValidType("image")) throw new WrongTypeException("Wrong file type");
        }
        if(vm.HoverImageFile != null)
        {
            if (!vm.HoverImageFile.isValidSize(2)) throw new SizeLimitException("Image size cannot be greater than 2 mb");
            if (!vm.HoverImageFile.isValidType("image")) throw new WrongTypeException("Wrong file type");
        }
        if(vm.ImageFiles != null)
        {
            foreach (var item in vm.ImageFiles)
            {
                if (!item.isValidSize(2)) throw new SizeLimitException("Image size cannot be greater than 2 mb: " + item.Name);
                if (!item.isValidType("image")) throw new WrongTypeException("Wrong file type: " + item.Name);
            }
        }
        if (!ModelState.IsValid)
        {
            return View();
        }
        await _productService.Create(vm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ChangeStatus(int id)
    {
        await _productService.SoftDelete(id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id <= 0 || id == null) return BadRequest();
        var entity = await _productService.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p=>p.Id == id);
        if (entity == null) return NotFound();
        UpdateProductVM vm = new UpdateProductVM
        {
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Discount = entity.Discount,
            Rating = entity.Rating,
            StockCount = entity.StockCount,
            ProductImages = entity.ProductImages,
            HoverImage = entity.HoverImage,
            MainImage = entity.MainImage
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, UpdateProductVM vm)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _productService.GetByIdAsync(id);
        if (entity == null) return NotFound();
        await _productService.Update(id, vm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> DeleteImage(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        await _productService.DeleteImage(id);
        return Ok();
    }
}

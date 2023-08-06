using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.CategoryVMs;

namespace Pronia.Areas.Manage.Controllers;
[Area("Manage")]
public class CategoryController : Controller
{
    private readonly ICategoryService _catService;

    public CategoryController(ICategoryService catService)
    {
        _catService = catService;
    }

    public async Task<IActionResult> Index() => View(await _catService.GetAll());
    public IActionResult Create() => View();
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryVM vm)
    {
        if (String.IsNullOrEmpty(vm.Name) || String.IsNullOrWhiteSpace(vm.Name)) throw new NullReferenceException();
        await _catService.Create(vm);
        return RedirectToAction(nameof(Index));
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.SliderVMs;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly ISliderService _service;
        public SliderController(ISliderService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM sliderVM)
        {
            if (sliderVM.ImageFile != null)
            {
                if (!sliderVM.ImageFile.ContentType.StartsWith("image/"))
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                if (sliderVM.ImageFile.Length > 1024 * 1024 * 2)
                    ModelState.AddModelError("ImageFile", "Max size is 2mb");
            }
            if (!ModelState.IsValid) return View();
            await _service.Create(sliderVM);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            await _service.Delete(id);
            TempData["isDeleted"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();
            var entity = await _service.GetById(id);
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSliderVM sliderVm)
        {
            await _service.Update(sliderVm);
            return RedirectToAction(nameof(Index));
        }
    }
}

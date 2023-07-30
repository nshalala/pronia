using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.ViewModels.SliderVMs;

namespace Pronia.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly ProniaDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(ProniaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
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
            using FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets", "imgs",
                sliderVM.ImageFile.FileName), FileMode.Create);
            await sliderVM.ImageFile.CopyToAsync(fs);
            await _context.Sliders.AddAsync(new Slider
            {
                ImageUrl = sliderVM.ImageFile.FileName,
                Title = sliderVM.Title,
                Offer = sliderVM.Offer,
                Description = sliderVM.Description,
                ButtonText = sliderVM.ButtonText
            }); ;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id == null) return BadRequest();
            var entity = await _context.Sliders.FindAsync(id);
            if (entity == null) return NotFound();
            _context.Sliders.Remove(entity);
            await _context.SaveChangesAsync();
            TempData["isDeleted"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (!ModelState.IsValid) return View();
            if (id < 1 || id == null) return BadRequest();
            var entity = await _context.Sliders.FindAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id < 1 || id == null || id != slider.Id) return BadRequest();
            var entity = await _context.Sliders.FindAsync(id);
            if (entity == null) return NotFound();
            entity.Title = slider.Title;
            entity.Description = slider.Description;
            entity.Offer = slider.Offer;
            entity.ButtonText = slider.ButtonText;
            entity.ImageUrl = slider.ImageUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

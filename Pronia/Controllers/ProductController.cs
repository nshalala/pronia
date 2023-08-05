using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Helpers.Exceptions;
using Pronia.Services.Interfaces;

namespace Pronia.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _prodService;

        public ProductController(IProductService prodService)
        {
            _prodService = prodService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id <= 0 || id == null) return BadRequest();
            var entity = await _prodService.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (entity == null) return NotFound();
            return View(entity);
        }
    }
}

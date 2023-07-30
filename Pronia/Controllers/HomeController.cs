using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProniaDbContext _context;
        public HomeController(ProniaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
    }
}
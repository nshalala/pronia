using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.SliderVMs;

namespace Pronia.Services.Implementations;

public class SliderService : ISliderService
{
    private readonly ProniaDbContext _context;
    private readonly IWebHostEnvironment _env;
    public SliderService(ProniaDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task Create(CreateSliderVM sliderVm)
    {
        using FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets", "imgs",
                sliderVm.ImageFile.FileName), FileMode.Create);
        await sliderVm.ImageFile.CopyToAsync(fs);
        await _context.Sliders.AddAsync(new Slider
        {
            ImageUrl = sliderVm.ImageFile.FileName,
            Title = sliderVm.Title,
            Offer = sliderVm.Offer,
            Description = sliderVm.Description,
            ButtonText = sliderVm.ButtonText
        }); ;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int? id)
    {
        var entity = await GetById(id);
        _context.Sliders.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Slider>> GetAll()
    {
        return await _context.Sliders.ToListAsync();
    }

    public async Task<Slider> GetById(int? id)
    {
        if (id < 1 || id == null) throw new ArgumentException();
        var entity = await _context.Sliders.FindAsync(id);
        if (entity == null) throw new ArgumentNullException();
        return entity;
    }

    public async Task Update(UpdateSliderVM sliderVm)
    {
        var entity = await GetById(sliderVm.Id);
        entity.Title = sliderVm.Title;
        entity.Description = sliderVm.Description;
        entity.Offer = sliderVm.Offer;
        entity.ButtonText = sliderVm.ButtonText;
        //entity.ImageUrl = sliderVm.ImageUrl;
        await _context.SaveChangesAsync();
    }
}

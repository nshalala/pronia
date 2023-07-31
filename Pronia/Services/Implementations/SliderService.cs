using Microsoft.EntityFrameworkCore;
using Pronia.DataAccess;
using Pronia.ExtensionServices.Interfaces;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels.SliderVMs;

namespace Pronia.Services.Implementations;

public class SliderService : ISliderService
{
    private readonly ProniaDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IFileService _fileService;
    public SliderService(ProniaDbContext context, IWebHostEnvironment env, IFileService fileService)
    {
        _context = context;
        _env = env;
        _fileService = fileService;
    }
    public async Task Create(CreateSliderVM sliderVm)
    {
        await _context.Sliders.AddAsync(new Slider
        {
            ImageUrl = await _fileService.UploadAsync(sliderVm.ImageFile, Path.Combine("assets", "imgs"), 
            "image", 2),
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

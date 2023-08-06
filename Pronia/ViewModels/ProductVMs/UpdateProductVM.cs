using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.ProductVMs;

public record UpdateProductVM
{
    [Required, MaxLength(64)]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required, Range(0, Double.MaxValue)]
    public decimal Price { get; set; }
    [Required, Range(0, 100)]
    public byte Discount { get; set; }
    [Required, Range(0, 5)]
    public byte Rating { get; set; }
    [Required, Range(0, Int32.MaxValue)]
    public int StockCount { get; set; }
    public string? MainImage { get; set; }
    public string? HoverImage { get; set; }
    public ICollection<ProductImage>? ProductImages { get; set; }
    public IFormFile MainImageFile { get; set; }
    public IFormFile? HoverImageFile { get; set; }
    public ICollection<IFormFile>? ProductImageFiles { get; set; }
}

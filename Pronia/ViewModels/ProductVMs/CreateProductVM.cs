﻿using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.ProductVMs;

public record CreateProductVM
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
    [Required]
    public IFormFile MainImageFile { get; set; }
    public IFormFile? HoverImageFile { get; set; }
    public ICollection<IFormFile>? ImageFiles { get; set; }
}

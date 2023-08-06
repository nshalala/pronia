using System.ComponentModel.DataAnnotations;

namespace Pronia.ViewModels.CategoryVMs;

public class CreateCategoryVM
{
    [Required]
    public string Name { get; set; }
}

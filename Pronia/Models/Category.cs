using System.ComponentModel.DataAnnotations;

namespace Pronia.Models;

public class Category:BaseEntity
{
    [Required, MaxLength(30)]
    public string Name { get; set; }
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}

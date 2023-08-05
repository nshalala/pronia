using Pronia.Models;

namespace Pronia.ViewModels.HomeVMs
{
    public class HomeVM
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<Slider> Sliders { get; set; }
    }
}

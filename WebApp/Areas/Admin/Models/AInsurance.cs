using WebApp.Models;
namespace WebApp.Areas.Admin.Models
{
    public class AInsurance : Insurance
    {
        public IFormFile? ImageFile { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TixtlySW.Areas.Cliente.Controllers
{
    [Area("Administrador")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

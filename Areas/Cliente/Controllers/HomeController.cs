using Microsoft.AspNetCore.Mvc;

namespace TixtlySW.Areas.Empleado.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
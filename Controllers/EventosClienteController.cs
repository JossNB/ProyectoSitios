using Microsoft.AspNetCore.Mvc;
using TixtlySW.Datos;
using TixtlySW.Models;

namespace TixtlySW.Areas.Cliente.Controllers
{
    public class EventosClienteController : Controller
    {

        EventoDatos _EventoDatos= new EventoDatos();
        public IActionResult Listar()
        {
            //Mostrara todos los datos 
            //var olista= _EventoDatos.Listar();
            //return View(//olista//);
            return View();

        }

        public IActionResult Filtrar()
        {
            //Mostrara datos filtrados 
            return View();
        }
    }
}

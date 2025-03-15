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
            //return (olista);
            return View();

        }

        public IActionResult Filtrar()
        {
            //Mostrara datos filtrados 
            var olista = _EventoDatos.Listar();
            return View(olista);
        }
    


        public IActionResult ObtenerEventoid(int eventoId)
        {
        // Llamar al método BuscarEventoporId
        var evento = _EventoDatos.BuscarEventoporId(eventoId);

        if (evento == null)
        {
            // Si no se encuentra el evento, puedes redirigir a una vista de error o mostrar un mensaje
            return NotFound(); // Retorna un error 404
        }

        // Pasar el evento encontrado a la vista
        return View(evento);
        }
    }
}

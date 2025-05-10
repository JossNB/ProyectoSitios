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
        }//TERMINA EL FILRAR 



		[HttpPost]
		public IActionResult Filtrar([FromBody] FiltrosBusqueda filtros)
		{
			try
			{
				// Validar los filtros
				if (filtros == null)
				{
					return BadRequest("Los filtros no pueden ser nulos.");
				}

				Console.WriteLine("Filtros recibidos:");
				Console.WriteLine($"Categoria: {filtros.Categoria}");
				Console.WriteLine($"Lugar: {filtros.Lugar}");
				Console.WriteLine($"FechaInicio: {filtros.FechaInicio}");
				Console.WriteLine($"FechaFin: {filtros.FechaFin}");
				Console.WriteLine($"Precio: {filtros.Precio}");

				// Convertir las fechas a DateTime o NULL
				DateTime? fechaInicio = null;
				DateTime? fechaFin = null;

				if (!string.IsNullOrEmpty(filtros.FechaInicio))
				{
					fechaInicio = DateTime.Parse(filtros.FechaInicio);
				}

				if (!string.IsNullOrEmpty(filtros.FechaFin))
				{
					fechaFin = DateTime.Parse(filtros.FechaFin);
				}

				// Crear un objeto FiltrosBusqueda con los valores correctos
				var filtrosBusqueda = new FiltrosBusqueda
				{
					Categoria = filtros.Categoria,
					Lugar = filtros.Lugar,
					FechaInicio = fechaInicio?.ToString("yyyy-MM-dd"), // Convertir a string en formato ISO
					FechaFin = fechaFin?.ToString("yyyy-MM-dd"), // Convertir a string en formato ISO
					Precio = filtros.Precio
				};

				// Llamar al método de la capa de datos
				var resultados = _EventoDatos.FiltrarEventos(filtros);

				Console.WriteLine($"Número de resultados: {resultados.Count}");

				// Devolver los resultados en formato JSON
				return Json(resultados);
			}
			catch (Exception ex)
			{
				// Registrar el error
				Console.Error.WriteLine($"Error en Filtrar: {ex.Message}");
				return StatusCode(500, "Ocurrió un error interno al procesar la solicitud.");
			}
		}//termina filtrar eventos 

		public class FiltrosBusqueda
		{
			public int? Categoria { get; set; } // Debe ser  nullable (int?)
			public string Lugar { get; set; }
			public string FechaInicio { get; set; } // Usar string para manejar fechas como texto
			public string FechaFin { get; set; } // Usar string para manejar fechas como texto
			public string Precio { get; set; }
		}//TERMINA LA CLASE FILTRO


		public IActionResult ObtenerEventoid(int eventoId)
        {
            //Obtener el evento (un solo registro)
            var evento = _EventoDatos.BuscarEventoporId(eventoId);

            if (evento == null)
            {
                // Si no se encuentra el evento, puedes redirigir a una vista de error o mostrar un mensaje
                return NotFound(); // Retorna un error 404
            }

            // Obtener la lista de boletos asociados al evento
            var boletos = _EventoDatos.BuscarBoletoEventoporId(eventoId);

            // Creas una instancia del ViewModel y asignas los datos
            var viewModel = new EventoBoletoViewModel
            {
                Evento = evento,
                Boletos = boletos
            };

            // Pasas el ViewModel a la vista
            return View(viewModel);
        }//termina el metodo obtenerEventoId

        public IActionResult BuscarBoleto(int eventoId)
        {
            try
            {
                // Obtener el evento (un solo registro)
                var evento = _EventoDatos.BuscarEventoporId(eventoId);

                if (evento == null)
                {
                    // Si no se encuentra el evento, 
                    return NotFound(); // Retorna un error 404
                }

                // Obtener la lista de boletos asociados al evento
                var boletos = _EventoDatos.BuscarBoletoEventoporId(eventoId);

                // Obtener la cantidad de sectores y filas usando el procedimiento almacenado
                var sectoresAsientos = _EventoDatos.ObtenerBloquesyAsientos(eventoId);

                // Crear una instancia del ViewModel adicional y asignar los datos
                var viewModel = new EventoBoletoConSectoresViewModel
                {
                    Evento = evento,
                    Boletos = boletos,
                    SectoresAsientos = sectoresAsientos
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Registrar el error para depuración
                Console.WriteLine("Error en BuscarBoleto: " + ex.Message);
                return StatusCode(500, "Ocurrió un error interno. Por favor, inténtelo de nuevo más tarde.");
            }
        }//termina el buscar boleto 


    }//termina la clase publica 
}//termina el controller 

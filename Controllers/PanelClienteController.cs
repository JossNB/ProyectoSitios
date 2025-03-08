using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TixtlySW.Controllers
{
    public class PanelClienteController : Controller
    {
        public IActionResult MenuPrincipal()
        {
            return View();
        }

        private readonly IConfiguration _configuration;

        public PanelClienteController(IConfiguration configuration)
        {
            _configuration = configuration; // Asignamos la configuración
        }


        public IActionResult ObtenerFondoMenu()
        {
            string? connectionString = _configuration.GetConnectionString("conexion");
            string procedimientoAlmacenado = "ObtenerFondoMenu";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(procedimientoAlmacenado, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    byte[] imagenBytes = reader["Imagen"] as byte[];

                    if (imagenBytes != null)
                    {
                        return File(imagenBytes, "image/jpg"); // Devuelve la imagen como archivo
                    }
                }
            }

            return NotFound(); // Si no hay imagen, devuelve un error 404
        }
    }
}

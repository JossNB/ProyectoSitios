using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TixtlySW.Models;
using System.IO;

namespace TixtlySW.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClienteController(IConfiguration configuration)
        {
            _configuration = configuration; // Asignamos la configuración
        }

        public IActionResult RegistroCliente()
        {
            return View();
        }

        public IActionResult ObtenerLogoSitio()
        {
            string? connectionString = _configuration.GetConnectionString("conexion");
            string procedimientoAlmacenado = "ObtenerLogo";

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
                        return File(imagenBytes, "image/png"); // Devuelve la imagen como archivo
                    }
                }
            }

            return NotFound(); // Si no hay imagen, devuelve un error 404
        }

        public IActionResult InicioSesion()
        {
            return View();
        }

        public IActionResult ObtenerFondoInicio()
        {
            string? connectionString = _configuration.GetConnectionString("conexion");
            string procedimientoAlmacenado = "ObtenerFondoIS";

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

        public IActionResult RecuperacionVista()
        {
            return View();
        }

        public IActionResult RecuperacionContrasenia()
        {
            return View();
        }
    }
}

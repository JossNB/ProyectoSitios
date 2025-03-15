using TixtlySW.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace TixtlySW.Datos
{
    public class EventoDatos
    {
        public List<EventoModel> Listar()
        {
            var olista = new List<EventoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                // Abrir la conexión y ejecutar el procedimiento almacenado
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerEventosConLugar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Obtener la información
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Manejo de imágenes
                        byte[] imagenBytes = null;
                        if (dr["Imagen"] != DBNull.Value) // Verificar si la imagen no es nula
                        {
                            imagenBytes = (byte[])dr["Imagen"];
                        }

                        // Convertir el arreglo de bytes a una URL de datos
                        string imagenUrl = null;
                        if (imagenBytes != null)
                        {
                            string base64String = Convert.ToBase64String(imagenBytes);
                            imagenUrl = $"data:image/jpeg;base64,{base64String}";
                        }

                        olista.Add(new EventoModel()
                        {
                            EventoID = Convert.ToInt32(dr["EventoID"]),
                            Titulo = dr["Titulo"].ToString(),
                            CategoriaID = Convert.ToInt32(dr["CategoriaEvento"]),
                            Descripcion = dr["Descripcion"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                            Estado = dr["Estado"].ToString(),
                            Imagen = imagenBytes,
                            ImagenUrl = imagenUrl, // Asignar la URL de la imagen
                            NombreLugar = dr["NombreLugar"].ToString(),
                            DireccionLugar = dr["DireccionLugar"].ToString()
                        }); // Termina el Add olista
                    } // Termina el while
                } // Termina el using dr
            } // Termina el using de la conexión

            return olista;
        }// termina el listar 


        public EventoModel BuscarEventoporId(int eventoId)
        {
            EventoModel evento = null; // Inicializar el objeto EventoModel

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                // Abrir la conexión y ejecutar el procedimiento almacenado
                conexion.Open();
                SqlCommand cmd = new SqlCommand("BuscarEventoporID", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar el parámetro @EventoID
                cmd.Parameters.AddWithValue("@EventoID", eventoId);

                // Obtener la información
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) // Si se encuentra el evento
                    {
                        // Manejo de imágenes
                        byte[] imagenBytes = null;
                        if (dr["Imagen"] != DBNull.Value) // Verificar si la imagen no es nula
                        {
                            imagenBytes = (byte[])dr["Imagen"];
                        }

                        // Convertir el arreglo de bytes a una URL de datos
                        string imagenUrl = null;
                        if (imagenBytes != null)
                        {
                            string base64String = Convert.ToBase64String(imagenBytes);
                            imagenUrl = $"data:image/jpeg;base64,{base64String}";
                        }

                        // Crear el objeto EventoModel
                        evento = new EventoModel()
                        {
                            EventoID = Convert.ToInt32(dr["EventoID"]),
                            LugarID = Convert.ToInt32(dr["LugarID"]), // Asegúrate de que esta columna exista en el resultado
                            CategoriaID = Convert.ToInt32(dr["CategoriaEvento"]),
                            Titulo = dr["Titulo"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaFin = Convert.ToDateTime(dr["FechaFin"]),
                            Estado = dr["Estado"].ToString(),
                            Imagen = imagenBytes,
                            ImagenUrl = imagenUrl, // Asignar la URL de la imagen
                            NombreLugar = dr["NombreLugar"].ToString(),
                            DireccionLugar = dr["DireccionLugar"].ToString()
                        };
                    }
                } // Termina el using dr
            } // Termina el using de la conexión

            return evento; // Retornar el evento encontrado (o null si no se encontró)
        } //BuscarEventoporId

        public List<BoletoEventoModel> BuscarBoletoEventoporId(int eventoId)
        {
            var listaBE = new List<BoletoEventoModel>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                // Abrir la conexión y ejecutar el procedimiento almacenado
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerEventoBoletosPorEventoID", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                // Obtener la información
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {


                        listaBE.Add(new BoletoEventoModel()
                        {
                            EventoID = Convert.ToInt32(dr["EventoID"]),
                            CategoriaID = Convert.ToInt32(dr["CategoriaID"]),
                            Precio= Convert.ToDecimal(dr["Precio"]),
                            TotalDisponibles= Convert.ToInt32(dr["TotalDisponibles"]),
                            Disponibles = Convert.ToInt32(dr["Disponibles"]),
                            IDsector = Convert.ToInt32(dr["IDsector"]),
                            CantidadMaximaCompra = Convert.ToInt32(dr["CantidadMaximaCompra"]),
                        }); // Termina el Add olista
                    } // Termina el while
                } // Termina el using dr
            } // Termina el using de la conexión

            return listaBE;
        }//termina BuscarBoletoEventoporId


    }
}

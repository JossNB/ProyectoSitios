using TixtlySW.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using static TixtlySW.Areas.Cliente.Controllers.EventosClienteController;


namespace TixtlySW.Datos
{
    public class EventoDatos
    {
        //metodo para obtener todos los eventos y pintarlos en la pagina de inicio de busqueda
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

        //Busca la informacion de un evento de acuerdo con su id

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


        //busca la lista de precios y sectores de un evento, de acuerdo con el id del evento
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
                // Agregar el parámetro @EventoID
                cmd.Parameters.AddWithValue("@EventoID", eventoId);

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
							NombreSector=dr["NombreBloque"].ToString(),
							CantidadMaximaCompra = Convert.ToInt32(dr["CantidadMaximaCompra"]),
                        }); // Termina el Add olista
                    } // Termina el while
                } // Termina el using dr
            } // Termina el using de la conexión

            return listaBE;
        }//termina BuscarBoletoEventoporId


		//aplica filtros dinamicos para buscar eventos
		public List<EventoModel> FiltrarEventos(FiltrosBusqueda filtros)
		{
			var olista = new List<EventoModel>();

			var cn = new Conexion();

			using (var conexion = new SqlConnection(cn.getCadenaSQL()))
			{
				// Abrir la conexión y ejecutar el procedimiento almacenado
				conexion.Open();
				SqlCommand cmd = new SqlCommand("ObtenerEventosFiltrados", conexion);
				cmd.CommandType = CommandType.StoredProcedure;

				// Agregar los parámetros al procedimiento almacenado
				cmd.Parameters.AddWithValue("@CategoriaEvento", (object)filtros.Categoria ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Direccion", (object)filtros.Lugar ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaInicio", (object)filtros.FechaInicio ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaFin", (object)filtros.FechaFin ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@RangoPrecio", (object)filtros.Precio ?? DBNull.Value);

				Console.WriteLine("Parámetros enviados al procedimiento almacenado:");
				Console.WriteLine($"@CategoriaEvento: {filtros.Categoria}");
				Console.WriteLine($"@Direccion: {filtros.Lugar}");
				Console.WriteLine($"@FechaInicio: {filtros.FechaInicio}");
				Console.WriteLine($"@FechaFin: {filtros.FechaFin}");
				Console.WriteLine($"@RangoPrecio: {filtros.Precio}");

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
							CategoriaID = Convert.ToInt32(dr["CategoriaEvento"]), // Asegúrate de que coincida con el nombre de la columna
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

			Console.WriteLine($"Número de eventos encontrados: {olista.Count}");

			return olista;
		}//termina el filtrar eventos 


        //evento para obtener la cantidad de sectores y asientos y pintar el html de manera dinamica 
        public List<CantidadSectoresAsientos> ObtenerBloquesyAsientos(int eventoID)
        {
            var listaSectoresAsientos = new List<CantidadSectoresAsientos>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                try
                {
                    // Abrir la conexión y ejecutar el procedimiento almacenado
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("ObtenerBloquesyAsientos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar el parámetro @EventoID
                    cmd.Parameters.AddWithValue("@EventoID", eventoID);

                    // Obtener la información
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaSectoresAsientos.Add(new CantidadSectoresAsientos()
                            {
                                IDSector = Convert.ToInt32(dr["IDSector"]),  
                                CantidadAsientos = Convert.ToInt32(dr["CantidadAsientos"]), 
                                CantidadFilas = Convert.ToInt32(dr["CantidadFilas"]),  
                                NombreBloque = dr["NombreBloque"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Registrar el error para depuración
                    Console.WriteLine("Error en ObtenerBloquesyAsientos: " + ex.Message);
                    throw; // Relanzar la excepción
                }
            }

            return listaSectoresAsientos;
        }//termina el buscar cantidad de bloques y asientos 


    }
}

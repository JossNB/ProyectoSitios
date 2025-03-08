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
            var olista=new List<EventoModel>();

            var cn= new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                //Abrir la conexion y ejecutar el procedimiento almacenado
                conexion.Open();
                SqlCommand cmd = new SqlCommand("ObtenerEventosConLugar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                //Obtener la informacion 
                using ( var dr= cmd.ExecuteReader())
                {
                    while (dr.Read()) {
                        olista.Add(new EventoModel()
                        {
                            EventoID = Convert.ToInt32(dr["E.EventoID"]),
                            LugarID = Convert.ToInt32(dr["NombreLugar"]),
                            CategoriaID = Convert.ToInt32(dr["E.CategoriaID"]),
                            Titulo  = dr["E.Titulo"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["E.FechaInicio"]), // Convertir a DateTime
                            FechaFin = Convert.ToDateTime(dr["E.FechaFin"]),       // Convertir a DateTime
                            Estado = dr["E.Estado"].ToString(),
                            Descripcion = dr["E.Descripcion"].ToString(),
                            ImagenUrl = dr["E.ImagenUrl"].ToString(),
                            NombreLugar = dr["NombreLugar"].ToString(),
                            DireccionLugar = dr["DireccionLugar"].ToString()
                        });//termina el add olista
                    }//termina el while
                }//termina el using dr
            }//termina el using de la conexion 

            return olista;
        }
  
    }
}

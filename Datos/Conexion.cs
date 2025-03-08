using System.Data.SqlClient;


namespace TixtlySW.Datos
{
    public class Conexion
    {
        private string cadenaSQL = string.Empty;

        //Constructor 
        public Conexion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            cadenaSQL = builder.GetSection("ConnectionStrings:conexion").Value;
        }//termina el constructor 

        //Obtener la cadena SQL
        public string getCadenaSQL ()
        {
            return cadenaSQL;
        }

    }//termina la clase 
}

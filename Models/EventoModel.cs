namespace TixtlySW.Models
{
    public class EventoModel
    {
        public int EventoID { get; set; }
        public int LugarID { get; set; }

        public int CategoriaID { get; set; }

        public string Titulo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public String Descripcion { get; set; } 

        public string Estado { get; set; }

        public string ImagenUrl { get; set; }

        public string NombreLugar { get; set; }
        public string DireccionLugar { get; set; }




    }
}

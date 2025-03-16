using NuGet.Versioning;

namespace TixtlySW.Models
{
    public class BoletoEventoModel
    {
        public int EventoBoletoID { get; set; }
        public int EventoID { get; set; }
        public int CategoriaID { get; set; }
        public decimal Precio { get; set; }
        public int TotalDisponibles { get; set; }
        public int Disponibles { get; set; }
        public int CantidadMaximaCompra { get; set; }
        public int IDsector { get; set; }

        public string NombreSector { get; set; }


    }
}

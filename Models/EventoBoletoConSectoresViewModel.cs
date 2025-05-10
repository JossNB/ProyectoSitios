namespace TixtlySW.Models
{
    public class EventoBoletoConSectoresViewModel
    {
        public EventoModel Evento { get; set; }
        public List<BoletoEventoModel> Boletos { get; set; }
        public List<CantidadSectoresAsientos> SectoresAsientos { get; set; }
    }
}

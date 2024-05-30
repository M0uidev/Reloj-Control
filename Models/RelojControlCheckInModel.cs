using System.ComponentModel;

namespace Reloj_Control.Models
{
    public class RelojControlCheckInModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Hora Entrada")]
        public DateTime HoraEntrada { get; set; }

        public RelojControlCheckInModel()
        {
            HoraEntrada = DateTime.Now; // Current time
        }
    }
}

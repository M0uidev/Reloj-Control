using System.ComponentModel;

namespace Reloj_Control.Models
{
    public class RelojControlCheckOutModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Hora Salida")]
        public DateTime HoraSalida { get; set; }

        public RelojControlCheckOutModel()
        {
            HoraSalida = DateTime.Now; // Current time
        }
    }
}

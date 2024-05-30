using System.ComponentModel;

namespace Reloj_Control.Models
{
    public class UserModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Nombre De Usuario")]
        public required string UserName { get; set; }
        [DisplayName("Contraseña")]
        public required string Password { get; set; }
    }
}

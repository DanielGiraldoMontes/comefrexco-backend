using System.ComponentModel.DataAnnotations;

namespace ComeFrexco.Models
{
    public class Usuario
    {
        [Required]
        [MaxLength(20, ErrorMessage = "El usuario no tiene esa longitud")]
        public string userName { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string isAdmin { get; set; }
    }
    public class Permissions
    {
        public string userName { get; set; }
        public string module { get; set; }
        public string permission { get; set; }
        public int idPermission { get; set; }
        public string grante { get; set; }
        public string label { get; set; }
    }
}

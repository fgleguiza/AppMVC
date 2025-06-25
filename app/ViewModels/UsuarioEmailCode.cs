using System.ComponentModel.DataAnnotations;

namespace app.ViewModels
{
    public class UsuarioEmailCode
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        public string Codigo { get; set; }
    }
}

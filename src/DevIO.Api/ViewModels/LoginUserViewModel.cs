using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está num formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caractere(s)", MinimumLength = 6)]
        public string Password { get; set; }
    }
}

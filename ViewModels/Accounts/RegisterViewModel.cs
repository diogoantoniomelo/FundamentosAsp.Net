using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O Nome é é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O E-mail é é obrigatório")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido")]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EclipseWorksChallenge.Models
{
    public class ProjetoModel
    {
        [Required(ErrorMessage = "O campo {0} é requerido.")]
        public string NomeUsuario { get; set; } = default!;
    }
}

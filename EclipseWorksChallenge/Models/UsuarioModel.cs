using System.ComponentModel.DataAnnotations;

namespace EclipseWorksChallenge.Models
{
    public enum FuncaoEnum
    {
        Estagiario,
        Contador,
        Analista,
        Gerente
    }
    public class UsuarioModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; } = default!;
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [AllowedValues([FuncaoEnum.Estagiario, FuncaoEnum.Contador, FuncaoEnum.Analista, FuncaoEnum.Gerente],
            ErrorMessage = "O valor de {0} é inválido. Revise os valores permitidos.")]
        public FuncaoEnum Funcao { get; set; }
    }
}

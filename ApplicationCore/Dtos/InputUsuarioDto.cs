using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Application.Messages;

namespace Application.Dtos
{
    public enum FuncaoEnum
    {
        Estagiario,
        Contador,
        Analista,
        Gerente
    }
    public class InputUsuarioDto
    {
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string NomeUsuario { get; set; } = default!;

        [Required(ErrorMessage = Mensagens.Requerido)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FuncaoEnum Funcao { get; set; }
    }
}

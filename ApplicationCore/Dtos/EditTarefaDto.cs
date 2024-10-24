using Application.CustomValidators;
using Application.Messages;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class EditTarefaDto
    {
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string Titulo { get; set; } = default!;

        [Required(ErrorMessage = Mensagens.Requerido)]
        public string Descricao { get; set; } = default!;
        /// <summary>
        /// Status da tarefa.
        /// </summary>
        [Required(ErrorMessage = Mensagens.Requerido)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusEnum Status { get; set; }
        /// <summary>
        /// Vencimento da tarefa.
        /// </summary>
        [Required(ErrorMessage = Mensagens.Requerido)]
        public DateTime Vencimento { get; set; }
        [Required]
        public string NomeUsuario { get; set; } = default!;
    }
}

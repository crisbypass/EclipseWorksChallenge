using Application.CustomValidators;
using Application.Messages;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class InputTarefaDto
    {
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string Titulo { get; set; } = default!;

        [Required(ErrorMessage = Mensagens.Requerido)]
        public string Descricao { get; set; } = default!;
        /// <summary>
        /// Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
        /// </summary>
        /// <remarks>
        /// Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
        /// </remarks>
        [Required(ErrorMessage = Mensagens.Requerido)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PrioridadeEnum Prioridade { get; set; }
        /// <summary>
        /// Status da tarefa.
        /// </summary>
        /// <remarks>
        /// Não tenho a informação se o Status inicial da tarefa pode ser Concluido.
        /// Conforme o caso, comente ou descomente a DataAnnotation 'VerificarStatusInicial'.
        /// </remarks>
        [VerificarStatusInicial]
        [Required(ErrorMessage = Mensagens.Requerido)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusEnum Status { get; set; }
        /// <summary>
        /// Vencimento da tarefa.
        /// </summary>
        /// <remarks>
        /// Não tenho a informação se o Vencimento inicial da tarefa pode ser retroativo.
        /// Conforme o caso, comente ou descomente a DataAnnotation 'DataMinHoje'.
        /// </remarks>
        [DataMinHoje]
        [Required(ErrorMessage = Mensagens.Requerido)]
        public DateTime Vencimento { get; set; }
        [Required]
        public string NomeUsuario { get; set; } = default!;
    }
}

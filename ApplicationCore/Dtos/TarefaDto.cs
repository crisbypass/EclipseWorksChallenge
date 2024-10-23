using Domain.Enums;

namespace Application.Dtos
{
    public class TarefaDto
    {
        /// <summary>
        /// Fornecido pelo banco no momento da criação.
        /// </summary>
        public int Id { get; set; } 
        /// <summary>
        /// Valor recuperado a partir da rota.
        /// </summary>
        public int ProjetoId { get; set; }
        public string Titulo { get; set; } = default!;
        public string Descricao { get; set; } = default!;

        /// <summary>
        /// Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
        /// </summary>
        /// <remarks>
        /// Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
        /// </remarks>
        public PrioridadeEnum Prioridade { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime Vencimento { get; set; }
    }
}

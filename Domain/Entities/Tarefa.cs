using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// Uma tarefa é uma unidade de trabalho dentro de um projeto. Cada tarefa possui um título,
    /// uma descrição, uma data de vencimento e um status (pendente, em andamento, concluída).
    /// </summary>
    public class Tarefa : Entity
    {
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
        /// <summary>
        /// Comentarios individuais também podem historico.
        /// </summary>
        public ICollection<Comentario> Comentarios { get; set; } = default!;
        public StatusEnum Status { get; set; }
        public DateTime Vencimento { get; set; }
        public Projeto Projeto { get; set; } = default!;
        public ICollection<Historico> Historicos { get; set; } = default!;
    }
}

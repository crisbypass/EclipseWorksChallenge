using Domain.Entities;

namespace Application.Dtos
{
    public class HistoricoDto
    {
        public int TarefaId { get; set; }
        /// <summary>
        /// Descrição dos eventos efetuados no momento específico.
        /// </summary>
        public string Resumo { get; set; } = default!;
        /// <summary>
        /// Usuario mais recente a efetuar edições.
        /// </summary>
        public string Responsavel { get; set; } = default!;
        public DateTime DataAtualizacao { get; set; }
        public Tarefa Tarefa { get; set; } = default!;
    }
}

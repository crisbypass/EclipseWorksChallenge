namespace EclipseWorksChallenge.MyData.MyEntities
{
    public class Historico : Entity
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

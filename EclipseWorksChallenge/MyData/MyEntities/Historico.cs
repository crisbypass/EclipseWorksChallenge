namespace EclipseWorksChallenge.MyData.MyEntities
{
    public class Historico
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        /// <summary>
        /// Usuario mais recente a efetuar edições.
        /// </summary>
        public string Responsavel { get; set; } = default!;
        public DateTime DataAtualizacao { get; set; }
        public Tarefa Tarefa { get; set; } = default!;
    }
}

namespace Domain.Entities
{
    public class Comentario : Entity
    {
        public int TarefaId { get; set; }
        public string Descricao { get; set; } = default!;
        /// <summary>
        /// Usuario a efetuar o comentario.
        /// </summary>
        public string Responsavel { get; set; } = default!;
        public Tarefa Tarefa { get; set; } = default!;
    }
}

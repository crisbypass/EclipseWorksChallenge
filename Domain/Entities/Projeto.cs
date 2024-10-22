namespace Domain.Entities
{
    /// <summary> 
    /// Um projeto é uma entidade que contém várias tarefas.
    /// Um usuário pode criar, visualizar e gerenciar vários projetos.
    /// </summary>
    /// <remarks>
    /// Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
    /// Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar
    /// um erro e sugerir a conclusão ou remoção das tarefas primeiro.
    /// </remarks>
    public class Projeto : Entity
    {
        public string NomeUsuario { get; set; } = default!;
        /// <summary>
        /// Cada projeto tem um limite máximo de 20 tarefas. 
        /// Tentar adicionar mais tarefas do que o limite deveria resultar em um erro.
        /// </summary>
        /// <remarks>
        /// Vamos adicionar uma mensagem de validação para esse modelo.
        /// </remarks>
        public ICollection<Tarefa> Tarefas { get; set; } = [];
    }
}

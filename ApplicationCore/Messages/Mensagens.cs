namespace Application.Messages
{
    /// <summary>
    /// Para efeitos de simplicidade, foram usadas constantes simples, ao invés
    /// de um Objeto de Recursos(Resources).
    /// </summary>
    public class Mensagens
    {
        public const string Requerido = "O campo {0} é requerido.";
        public const string ProjetoCriado = "Projeto criado.";
        public const string FalhaCriacaoProjeto = "Falha na criação do projeto. Tente novamente mais tarde.";
        public const string ProjetoNaoEncontrado = "Projeto não encontrado.";
        public const string ProjetoRemovido = "Projeto removido.";
        public const string ErroExclusaoProjeto = "Não é possível excluir um projeto com tarefas ativas, somente após sua conclusão.";
        public const string LimiteTarefasAtingido = "O número limite de tarefas foi atingido para o projeto atual.";
        public const string TarefaCriada = "A tarefa foi criada.";
        public const string TarefaRemovida = "A tarefa foi removida.";
        public const string TarefaNaoEcontrada = "A tarefa não foi encontrada.";
        public const string DescricaoTarefaAlterada = "A descrição da tarefa foi alterada.";
        public const string StatusTarefaAlterado = "O status da tarefa foi alterado para {0}.";
        public const string TituloTarefaAlterado = "O título da tarefa foi alterado.";
        public const string VencimentoTarefaAlterado = "O vencimento da tarefa foi alterado.";
        public const string TarefaModificada = "A modificação da tarefa foi concluída.";
        public const string ComentarioTarefa = "Um comentário foi adicionado à tarefa.";
        public const string UsuarioNaoInformado = "Usuário não informado.";
    }
}

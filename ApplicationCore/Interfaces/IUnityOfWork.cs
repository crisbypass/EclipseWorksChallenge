using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnityOfWork
    {
        IGenericRepository<Comentario> ComentarioRepository { get; }
        IGenericRepository<Historico> HistoricoRepository { get; }
        IGenericRepository<Projeto> ProjetoRepository { get; }
        IGenericRepository<Tarefa> TarefaRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
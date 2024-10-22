using Application.Interfaces;
using Domain.Entities;
namespace Infrastructure.Data.Persistence.Repositories
{
    public class UnityOfWork(
        MyDbContext context,
        IGenericRepository<Projeto> projetoRepository,
        IGenericRepository<Tarefa> tarefaRepository,
        IGenericRepository<Historico> historicoRepository,
        IGenericRepository<Comentario> comentarioRepository) : IUnityOfWork
    {
        public IGenericRepository<Projeto> ProjetoRepository { get; } = projetoRepository;
        public IGenericRepository<Tarefa> TarefaRepository { get; } = tarefaRepository;
        public IGenericRepository<Historico> HistoricoRepository { get; } = historicoRepository;
        public IGenericRepository<Comentario> ComentarioRepository { get; } = comentarioRepository;

        private readonly MyDbContext _context = context;
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

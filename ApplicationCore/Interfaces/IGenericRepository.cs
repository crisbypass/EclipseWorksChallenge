using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> BuscarUnicoAsync(int keyId, Func<TEntity, object>[] propriedadesIncluidas = null!);
        Task<IEnumerable<TEntity>> BuscarVariosAsync(
            Expression<Func<TEntity, bool>> filtro = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordenarPor = null!,
            Func<TEntity, object>[] propriedadesIncluidas = null!,
            bool paginar = false,
            int pagina = 1,
            int tamanhoPagina = 100);
        Task<TEntity> EditarAsync(TEntity item);
        Task<TEntity> ExcluirAsync(int id);
        Task<TEntity> InserirAsync(TEntity item);
        Task<int> ContarItemsAsync(int? keyId = null, Expression<Func<TEntity, bool>> filtro = null!);
    }
}
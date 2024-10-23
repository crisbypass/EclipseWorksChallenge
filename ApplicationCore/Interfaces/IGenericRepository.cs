using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity?> BuscarUnicoAsync(int keyId, Func<TEntity, object>[] propriedadesIncluidas = null!);
        Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<TEntity> Items)> BuscarVariosAsync(
            Expression<Func<TEntity, bool>> filtro = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordenarPor = null!,
            bool paginate = true,
            int page = 1,
            int pageSize = 100,
            Func<TEntity, object>[] propriedadesIncluidas = null!
            );
        Task<TEntity> EditarAsync(TEntity item);
        Task<TEntity> ExcluirAsync(int id);
        Task<TEntity> InserirAsync(TEntity item);
        Task<int> ContarItemsAsync(int? keyId = null, Expression<Func<TEntity, bool>> filtro = null!);
    }
}
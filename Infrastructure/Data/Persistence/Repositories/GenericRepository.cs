using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Linq.Expressions;

namespace Infrastructure.Data.Persistence.Repositories
{

    public class GenericRepository<TEntity>(MyDbContext context) : IGenericRepository<TEntity>
        where TEntity : Entity
    {
        private readonly MyDbContext _context = context;
        public async Task<TEntity?> BuscarUnicoAsync(
            int keyId, Expression<Func<TEntity, object>>[] includedProperties = null!)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includedProperties != null)
            {
                foreach (var entidadeIncluida in includedProperties)
                {
                    query = query.Include(entidadeIncluida);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id == keyId);
        }
        public async Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<TEntity> Items)> BuscarVariosAsync(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            bool paginate = true,
            int page = 1,
            int pageSize = 10,
            Expression<Func<TEntity, object>>[] includedProperties = null!
            )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includedProperties != null)
            {
                foreach (var entidadeIncluida in includedProperties)
                {
                    query = query.Include(entidadeIncluida);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                var ordered = orderBy(query);

                if (paginate)
                {
                    var count = await ordered.CountAsync();
                    var totalPages = (int)Math.Ceiling(count / (double)pageSize);

                    bool hasPreviousPage = page > 1;
                    bool hasNextPage = page < totalPages;

                    var paginatedItems = await ordered
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    return (hasPreviousPage, hasNextPage, paginatedItems);
                }

                var items = await ordered.ToListAsync();

                return (false, false, items);
            }

            var resultItems = await query.ToListAsync();

            return (false, false, resultItems);
        }
        public async Task<IEnumerable<T>> BuscarVariosAsync<T>(Func<IQueryable<TEntity>, IQueryable<T>> query)
        {
            return await query(_context.Set<TEntity>()).ToListAsync();
        }
        public async Task<TEntity> InserirAsync(TEntity item)
        {
            _context.Set<TEntity>().Add(item);

            return await Task.FromResult(item);
        }
        public async Task<TEntity> EditarAsync(TEntity item)
        {
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Entry(item).State = EntityState.Modified;

            return await Task.FromResult(item);
        }
        public async Task<TEntity> ExcluirAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            if (entity != null)
            {
                if (_context.Set<TEntity>().Entry(entity).State == EntityState.Detached)
                {
                    _context.Set<TEntity>().Attach(entity);
                }

                _context.Set<TEntity>().Remove(entity);

                return entity;
            }
            return default!;
        }
        public async Task<int> ContarItensAsync(int? keyId = null!, Expression<Func<TEntity, bool>> filtro = null!)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filtro != null)
            {
                query.Where(filtro);
            }

            if (keyId != null)
            {
                return await query.CountAsync(x => x.Id == keyId);
            }

            return await query.CountAsync();
        }
    }
}

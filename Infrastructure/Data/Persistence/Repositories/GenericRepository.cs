using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Persistence.Repositories
{
    public class GenericRepository<TEntity>(MyDbContext context) : IGenericRepository<TEntity>
        where TEntity : Entity
    {
        private readonly MyDbContext _context = context;
        //public async Task<TEntity?> BuscarUnicoAsync(int keyId, string entidadesIncluidas = null!)
        //{
        //    IQueryable<TEntity> query = _context.Set<TEntity>();

        //    foreach (var entidadeIncluida in entidadesIncluidas.Split
        //        ([','], StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(entidadeIncluida);
        //    }

        //    return await query.FirstOrDefaultAsync(x => x.Id == keyId);
        //}
        public async Task<TEntity?> BuscarUnicoAsync(
            int keyId, Func<TEntity, object>[] propriedadesIncluidas = null!)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var entidadeIncluida in propriedadesIncluidas)
            {
                query = query.Include(x => entidadeIncluida(x));
            }

            return await query.FirstOrDefaultAsync(x => x.Id == keyId);
        }
        public async Task<IEnumerable<TEntity>> BuscarVariosAsync(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string entidadesIncluidas = null!,
            bool paginar = false,
            int pagina = 1,
            int tamanhoPagina = 100
            )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var entidadeIncluida in entidadesIncluidas.Split
                ([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(entidadeIncluida);
            }

            if (orderBy != null)
            {
                var ordered = orderBy(query);

                if (paginar)
                {
                    return await ordered
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .ToListAsync();
                }

                return await ordered.ToListAsync();
            }
            else
            {
                if (paginar)
                {
                    return await query
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .ToListAsync();
                }

                return await query.ToListAsync();
            }
        }
        public async Task<IEnumerable<TEntity>> BuscarVariosAsync(
            Expression<Func<TEntity, bool>> filtro = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordernarPor = null!,
            Func<TEntity, object>[] propriedadesIncluidas = null!,
            bool paginar = false,
            int pagina = 1,
            int tamanhoPagina = 100
            )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            foreach (var entidadeIncluida in propriedadesIncluidas)
            {
                query = query.Include(x => entidadeIncluida(x));
            }

            if (ordernarPor != null)
            {
                var ordered = ordernarPor(query);

                if (paginar)
                {
                    return await ordered
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .ToListAsync();
                }

                return await ordered.ToListAsync();
            }
            else
            {
                if (paginar)
                {
                    return await query
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .ToListAsync();
                }

                return await query.ToListAsync();
            }
        }
        public async Task<TEntity> InserirAsync(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
            //await _context.SaveChangesAsync();
            return await Task.FromResult(item);
        }
        public async Task<TEntity> EditarAsync(TEntity item)
        {
            //var entity = await _context.Set<TEntity>().FindAsync(item.Id);

            //if (entity != null)
            //{
                _context.Set<TEntity>().Attach(item);
                _context.Set<TEntity>().Entry(item).State = EntityState.Modified;

                //_context.Set<TEntity>().Update(item);
                //await _context.SaveChangesAsync();
                return await Task.FromResult(item);
            //}
            //return default!;
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

                //_context.Set<TEntity>().Remove(entity);

                //await _context.SaveChangesAsync();

                return entity;
            }
            return default!;
        }
        public async Task<int> ContarItemsAsync(int? keyId = null!, Expression<Func<TEntity, bool>> filtro = null!)
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
    public class PaginatedList<T>(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        public IReadOnlyCollection<T> Items { get; } = items;
        public int PageNumber { get; } = pageNumber;
        public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pageSize);
        public int TotalCount { get; } = count;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }


}

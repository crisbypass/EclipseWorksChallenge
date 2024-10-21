using EclipseWorksChallenge.MyData.MyEntities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EclipseWorksChallenge.MyData.MyRepositories
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
            int keyId,  Func<TEntity, object>[] propriedadesIncluidas = null!)
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
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<TEntity> EditarAsync(TEntity item)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == item.Id);

            if (entity != null)
            {
                _context.Set<TEntity>().Update(item);
                await _context.SaveChangesAsync();
                return item;
            }
            return default!;
        }
        public async Task<TEntity> ExcluirAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
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
}

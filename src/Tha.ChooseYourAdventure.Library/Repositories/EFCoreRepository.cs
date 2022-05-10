using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Library.Repositories
{
    public class EFCoreRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public EFCoreRepository(
            ApiDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<T> CreateAsync(T entity, CancellationToken cancellatonToken = default)
        {
            if (entity is IAuditable)
            {
                (entity as IAuditable).CreatedOn = System.DateTimeOffset.Now;
            }

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(cancellatonToken);

            return entity;
        }

        public async Task<T> DeleteAsync(object id, CancellationToken cancellatonToken = default)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) { return null; }
            
            if (entity is IAuditable)
            {
                (entity as IAuditable).UpdatedOn = System.DateTimeOffset.Now;
            }

            if (entity is ISoftDeletable)
            {
                (entity as ISoftDeletable).IsActive = false;
            }
            else
            {
                _context.Set<T>().Remove(entity);
            }

            await _context.SaveChangesAsync(cancellatonToken);
            return entity;
        }

        public IQueryable<T> Read()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<T> UpdateAsync(object id, T model, CancellationToken cancellatonToken = default)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) { return model; }
            _mapper.Map(model, entity);

            if (entity is IAuditable)
            {
                (entity as IAuditable).UpdatedOn = System.DateTimeOffset.Now;
            }

            await _context.SaveChangesAsync(cancellatonToken);
            return entity;
        }
    }
}

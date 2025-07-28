using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications;

namespace Store.Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private StoreDBContext _context;
        public GenericRepository(StoreDBContext context)
        {
            this._context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public void DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecifications(ISpecification<TEntity> specs)
        {
            return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>().AsQueryable(), specs).ToListAsync();
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> GetByIdWithSpecifications(ISpecification<TEntity> specs)
        {
            return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specs).FirstOrDefaultAsync();
        }

        public async Task<int> GetProductsCount(ISpecification<TEntity> specs)
        {
            return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), specs).CountAsync();
        }

        public void UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}

using System.Linq.Expressions;
using MongoDB.Driver;
using RouteScheduler.Core.Domain.Contracts;
using RouteScheduler.Infrastructure.Persistence.Data.Mongo;

namespace RouteScheduler.Infrastructure.Persistence.Repositories.Mongo
{
    public class MongoGenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoCollection<TEntity> _collection;

        public MongoGenericRepository(MongoContext context)
        {
            _collection = context.GetCollection<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            var filter = Builders<TEntity>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            var id = entity?.GetType().GetProperty("Id")?.GetValue(entity);
            if (id == null) throw new InvalidOperationException("Entity must have Id property for update.");
            var filter = Builders<TEntity>.Filter.Eq("Id", id);
            _collection.ReplaceOne(filter, entity);
        }

        public void Remove(TEntity entity)
        {
            var id = entity?.GetType().GetProperty("Id")?.GetValue(entity);
            if (id == null) throw new InvalidOperationException("Entity must have Id property for delete.");
            var filter = Builders<TEntity>.Filter.Eq("Id", id);
            _collection.DeleteOne(filter);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await _collection.CountDocumentsAsync(predicate);
            return count > 0;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return (int)await _collection.CountDocumentsAsync(Builders<TEntity>.Filter.Empty);
            return (int)await _collection.CountDocumentsAsync(predicate);
        }

        public async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var filter = predicate ?? (_ => true);
            var find = _collection.Find(filter);

            // orderBy not easily transferable; fallback to natural order
            var total = (int)await find.CountDocumentsAsync();
            var items = await find.Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
            return (items, total);
        }
    }
}





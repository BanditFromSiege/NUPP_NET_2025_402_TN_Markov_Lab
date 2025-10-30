using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilitaryVehicles.infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MilitaryVehiclesContext context;
        private readonly DbSet<T> dbSet;

        public Repository(MilitaryVehiclesContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
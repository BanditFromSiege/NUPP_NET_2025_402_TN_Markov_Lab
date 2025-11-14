using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.common;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MilitaryVehicles.common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly MilitaryVehiclesContext _context;

        public CrudServiceAsync(IRepository<T> repository, MilitaryVehiclesContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(Guid id)
        {
            return await _context.Set<T>()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id)
                ?? throw new KeyNotFoundException();
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await IncludeNavigation(_context.Set<T>()).ToListAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await IncludeNavigation(_context.Set<T>()).ToListAsync();
            return all.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.Update(element);
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.Delete(element);
            return true;
        }

        public Task<bool> SaveAsync() => Task.FromResult(true);
        public Task<bool> LoadAsync() => Task.FromResult(true);

        public IEnumerator<T> GetEnumerator() => ReadAllAsync().Result.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //method for include navigation properties
        private IQueryable<T> IncludeNavigation(DbSet<T> set)
        {
            if (typeof(T) == typeof(HelicopterModel))
            {
                return set.Cast<HelicopterModel>()
                          .Include(h => h.CrewMembers)
                          .AsQueryable() as IQueryable<T>;
            }
            else if (typeof(T) == typeof(DestroyerModel))
            {
                return set.Cast<DestroyerModel>()
                          .Include(d => d.CrewMembers)
                          .AsQueryable() as IQueryable<T>;
            }
            else if (typeof(T) == typeof(TankModel))
            {
                return set.Cast<TankModel>()
                          .Include(t => t.CrewMembers)
                          .AsQueryable() as IQueryable<T>;
            }
            else
            {
                return set;
            }
        }
    }
}
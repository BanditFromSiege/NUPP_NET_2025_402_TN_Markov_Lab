using MilitaryVehicles.common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using MilitaryVehicles.infrastructure;
using MilitaryVehicles.infrastructure.Models;

namespace MilitaryVehicles.common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : MilitaryVehicle
    {
        private readonly IRepository<T> repository;

        public CrudServiceAsync(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            try
            {
                await repository.AddAsync(element);
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} доданий");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при додаванні {element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id}: {ex.Message}");
                return false;
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            try
            {
                var element = await repository.GetByIdAsync((int)(object)id);
                if (element != null)
                    return element;

                throw new KeyNotFoundException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при читанні елемента з ідентифікатором {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            try
            {
                return await repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні списку {typeof(T).Name}: {ex.Message}");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            try
            {
                var all = await repository.GetAllAsync();
                return all.Skip((page - 1) * amount).Take(amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні сторінки {page} {typeof(T).Name}: {ex.Message}");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<bool> UpdateAsync(T element)
        {
            try
            {
                await repository.Update(element);
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} оновлено");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} не оновлено: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            try
            {
                await repository.Delete(element);
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} видалено");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося знайти {element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} для видалення: {ex.Message}");
                return false;
            }
        }

        public Task<bool> SaveAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> LoadAsync()
        {
            return Task.FromResult(true);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ReadAllAsync().Result.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
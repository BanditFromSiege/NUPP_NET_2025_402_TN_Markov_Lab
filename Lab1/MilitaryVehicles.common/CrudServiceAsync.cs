using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MilitaryVehicles.common
{
    public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : MilitaryVehicle
    {
        private readonly ConcurrentDictionary<Guid, T> elements = new ConcurrentDictionary<Guid, T>();
        private readonly string filePath;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public CrudServiceAsync(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task<bool> CreateAsync(T element)
        {
            if (elements.ContainsKey(element.Id))
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} вже існує");
                return false;
            }

            elements[element.Id] = element;
            Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} доданий");
            return await SaveAsync();
        }

        public async Task<T> ReadAsync(Guid id)
        {
            if (elements.TryGetValue(id, out T element))
            {
                return element;
            }

            throw new KeyNotFoundException($"Елемент з ідентифікатором {id} не знайдено");
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return elements.Values.ToList();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return elements.Values.Skip((page - 1) * amount).Take(amount).ToList();
        }

        public async Task<bool> UpdateAsync(T element)
        {
            if (elements.ContainsKey(element.Id))
            {
                elements[element.Id] = element;
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} оновлено");
                return await SaveAsync();
            }

            Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} не знайдено для оновлення");
            return false;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            if (elements.TryRemove(element.Id, out _))
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} видалено");
                return await SaveAsync();
            }

            Console.WriteLine($"Не вдалося знайти {element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} для видалення");
            return false;
        }

        public async Task<bool> SaveAsync()
        {
            await semaphore.WaitAsync();
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new MilitaryVehicleConverter() },
                    PropertyNamingPolicy = null,
                    PropertyNameCaseInsensitive = true
                };

                var list = elements.Values.ToList();

                string json = JsonSerializer.Serialize(list, options);

                await File.WriteAllTextAsync(filePath, json);
                Console.WriteLine("Дані збережені у файл");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні даних у файл: {ex.Message}");
                return false;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<bool> LoadAsync()
        {
            await semaphore.WaitAsync();
            try
            {
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    options.Converters.Add(new MilitaryVehicleConverter());

                    var loadedElements = JsonSerializer.Deserialize<List<T>>(json, options);

                    if (loadedElements != null)
                    {
                        elements.Clear();
                        foreach (var element in loadedElements)
                        {
                            elements[element.Id] = element;
                        }
                        Console.WriteLine("Дані завантажено з файлу");
                    }
                }
                else
                {
                    Console.WriteLine("Файл не знайдено, новий файл буде створено при збереженні");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні даних з файлу: {ex.Message}");
                return false;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return elements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public class CrudService<T> : ICrudService<T> where T : MilitaryVehicle
    {
        private readonly Dictionary<Guid, T> elements = new Dictionary<Guid, T>();

        public void Create(T element)
        {
            if (elements.ContainsKey(element.Id))
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} вже існує");
                return;
            }

            elements[element.Id] = element;
            Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} доданий");
        }

        public T Read(Guid id)
        {
            if (elements.TryGetValue(id, out T element))
            {
                return element;
            }

            throw new KeyNotFoundException($"Елемент з ідентифікатором {id} не знайдено");
        }

        public IEnumerable<T> ReadAll()
        {
            return elements.Values;
        }

        public void Update(T element)
        {
            if (elements.ContainsKey(element.Id))
            {
                elements[element.Id] = element;
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} оновлено");
            }
            else
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} не знайдено для оновлення");
            }
        }

        public void Remove(T element)
        {
            if (elements.Remove(element.Id))
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} видалено");
            }
            else
            {
                Console.WriteLine($"Не вдалося знайти {element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} для видалення");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryVehicles.common
{
    public interface ICrudService<T> where T : MilitaryVehicle
    {
        void Create(T element);
        T Read(Guid id);
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }

    public class CrudService<T> : ICrudService<T> where T : MilitaryVehicle
    {
        private readonly List<T> elements = new List<T>();

        public void Create(T element)
        {
            if (elements.Any(e => e.Id == element.Id))
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} вже існує");
                return;
            }

            elements.Add(element);
            Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} доданий");
        }

        public T Read(Guid id)
        {
            T element = elements.FirstOrDefault(e => e.Id == id);

            if (element == null)
            {
                throw new KeyNotFoundException($"Елемент з ідентифікатором {id} не знайдено");
            }

            return element;
        }

        public IEnumerable<T> ReadAll()
        {
            return elements;
        }

        public void Update(T element)
        {
            var index = elements.FindIndex(e => e.Id == element.Id);
            if (index != -1)
            {
                elements[index] = element;
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} оновлено");
            }
            else
            {
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} не знайдено для оновлення");
            }
        }

        public void Remove(T element)
        {
            bool removed = elements.Remove(element);
            if (removed)
                Console.WriteLine($"{element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} видалено");
            else
                Console.WriteLine($"Не вдалося знайти {element.GetType().Name} моделі {element.Model} з ідентифікатором {element.Id} для видалення");
        }
    }
}
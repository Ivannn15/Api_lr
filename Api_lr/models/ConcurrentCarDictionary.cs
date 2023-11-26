using inmemory.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace inmemory
{
    public class ConcurrentCarDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса car
        public ConcurrentDictionary<int, car> carDictionary;

        public ConcurrentCarDictionary()
        {
            carDictionary = new ConcurrentDictionary<int, car>();
        }

        // Методы для работы с ConcurrentDictionary

        // Добавление новой машины
        public void AddCar(car car)
        {
            carDictionary.TryAdd(car.Id, car);
        }

        // Получение машины по ID
        public car GetCar(int id)
        {
            car car;
            carDictionary.TryGetValue(id, out car);
            return car;
        }

        // Обновление информации о машине
        public void UpdateCar(car updatedCar)
        {
            carDictionary.AddOrUpdate(updatedCar.Id, updatedCar, (key, oldValue) => updatedCar);
        }

        // Удаление машины по ID
        public void DeleteCar(int id)
        {
            car removedCar;
            carDictionary.TryRemove(id, out removedCar);
        }

        internal IEnumerable<car> GetAllCar()
        {
            IEnumerable<car> allCars = carDictionary.Values;

            return allCars;
        }
    }

}

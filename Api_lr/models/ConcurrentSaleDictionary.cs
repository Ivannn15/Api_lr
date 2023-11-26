using inmemory.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory
{
    internal class ConcurrentSaleDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса sale
        private ConcurrentDictionary<int, sale> saleDictionary;

        public ConcurrentSaleDictionary()
        {
            saleDictionary = new ConcurrentDictionary<int, sale>();
        }

        // Методы для работы с ConcurrentDictionary

        // Добавление новой продажи
        public void AddSale(sale sale)
        {
            saleDictionary.TryAdd(sale.id, sale);
        }

        // Получение продажи по ID
        public sale GetSale(int id)
        {
            sale sale;
            saleDictionary.TryGetValue(id, out sale);
            return sale;
        }

        // Обновление информации о продаже
        public void UpdateSale(sale updatedSale)
        {
            saleDictionary.AddOrUpdate(updatedSale.id, updatedSale, (key, oldValue) => updatedSale);
        }

        // Удаление продажи по ID
        public void DeleteSale(int id)
        {
            sale removedSale;
            saleDictionary.TryRemove(id, out removedSale);
        }

        internal IEnumerable<sale> GetAllSales()
        {
            IEnumerable<sale> allSales = (IEnumerable<sale>)saleDictionary.Values;

            return (IEnumerable<sale>)allSales;
        }
    }
}

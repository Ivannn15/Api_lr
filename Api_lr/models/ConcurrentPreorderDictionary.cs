using inmemory.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory
{
    public class ConcurrentPreorderDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса preorder
        public ConcurrentDictionary<int, preorder> preorderDictionary;

        public ConcurrentPreorderDictionary()
        {
            preorderDictionary = new ConcurrentDictionary<int, preorder>();
        }

        // Методы для работы с ConcurrentDictionary

        // Добавление нового предзаказа
        public void AddPreorder(preorder preorder)
        {
            preorderDictionary.TryAdd(preorder.id, preorder);
        }

        // Получение предзаказа по ID
        public preorder GetPreorder(int id)
        {
            preorder preorder;
            preorderDictionary.TryGetValue(id, out preorder);
            return preorder;
        }

        // Обновление информации о предзаказе
        public void UpdatePreorder(preorder updatedPreorder)
        {
            preorderDictionary.AddOrUpdate(updatedPreorder.id, updatedPreorder, (key, oldValue) => updatedPreorder);
        }

        // Удаление предзаказа по ID
        public void DeletePreorder(int id)
        {
            preorder removedPreorder;
            preorderDictionary.TryRemove(id, out removedPreorder);
        }

        public IEnumerable<preorder> GetAllPreorders()
        {
            IEnumerable<preorder> allPreorders = (IEnumerable<preorder>)preorderDictionary.Values;

            return (IEnumerable<preorder>)allPreorders;
        }
    }

}

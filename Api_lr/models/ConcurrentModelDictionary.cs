using inmemory.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory
{
    public class ConcurrentModelDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса model
        public ConcurrentDictionary<int, model> modelDictionary;

        public ConcurrentModelDictionary()
        {
            modelDictionary = new ConcurrentDictionary<int, model>();
        }

        // Методы для работы с ConcurrentDictionary

        // Добавление новой модели
        public void AddModel(model model)
        {
            modelDictionary.TryAdd(model.Id, model);
        }

        // Получение модели по ID
        public model GetModel(int id)
        {
            model model;
            modelDictionary.TryGetValue(id, out model);
            return model;
        }

        // Обновление информации о модели
        public void UpdateModel(model updatedModel)
        {
            modelDictionary.AddOrUpdate(updatedModel.Id, updatedModel, (key, oldValue) => updatedModel);
        }

        // Удаление модели по ID
        public void DeleteModel(int id)
        {
            model removedModel;
            modelDictionary.TryRemove(id, out removedModel);
        }

        public IEnumerable<model> GetAllModel()
        {
            IEnumerable<model> allModels = modelDictionary.Values;

            return allModels;
        }
    }
}

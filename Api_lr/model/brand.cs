using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class brand
    {
        /// <summary>
        /// 1 Уникальный идентификатор бренда
        /// 2 Название бренда
        /// 3 Описание бренда
        /// 4 Фото бренда
        /// 
        /// Список автомобилей входящих в этот бренд
        /// Список моделей автомобилей входящих в этот бренд
        /// </summary>
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }

        public List<car> brands_car { get;set; }
        public List<model> brands_model { get; set; }
    }
}

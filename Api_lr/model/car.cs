using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class car
    {
        /// <summary>
        /// 1 Уникальный идентификатор автомобиля
        /// 2 Название автомобиля
        /// 3 Описание автомобиля
        /// 4 Фото автомобиля
        /// 5 Состояние автомобиля
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Type { get; set; }

        public int brandId { get; set; }
        public int modelId { get; set; }
        public int preorderid { get; set; }
        public int saleid { get; set; }
    }
}

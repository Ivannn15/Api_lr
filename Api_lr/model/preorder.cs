using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class preorder
    {
        /// <summary>
        /// 1 Уникальный идентификатор предзаказа
        /// 2 Цена
        /// 3 Дата предзаказа
        /// 4 Дата выдачи автомобиля
        /// 
        /// Список заказанных машин
        /// 5 Покупатель
        /// </summary>
        public int id { get; set; }
        public int price { get; set; }
        public DateTime time_start_order { get; set; }
        public DateTime time_end_order { get; set; }
        public List<car> Preorders { get; set; }
        public user user { get; set; }
    }
}

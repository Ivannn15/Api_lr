using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class sale
    {
        /// <summary>
        /// 1 Уникальный идентификатор продажи
        /// 2 Цена
        /// 3 Дата продажи
        /// 4 Список автомобилей входящих в продажу
        /// 5 Покупатель
        /// </summary>
        public int id { get; set; }
        public int price { get; set; }

        public DateTime time_start_order { get; set; }
        public user user { get; set; }
        public List<car> preorders { get; set; }
    }
}

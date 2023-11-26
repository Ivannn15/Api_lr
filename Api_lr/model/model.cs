using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class model
    {
        /// <summary>
        /// 1 Уникальный идентификатор модели
        /// 2 Название модели
        /// 3 Описание модели
        /// 4 Фото модели
        /// 
        /// Список машин этой модели
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int brandId { get; set; }

        public List<car> models_car { get; set; }
    }
}

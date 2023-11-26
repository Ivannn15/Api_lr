using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory.models
{
    public class user
    {
        /// <summary>
        /// 1 Уникальный идентификатор пользователя
        /// 2 Имя пользователя
        /// 3 Емаил пользователя
        /// 4 Пароль пользователя
        /// 5 Тип пользователя
        /// 
        /// Список покупок пользователя
        /// Список предзаказов пользователя
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }

        public List<sale> Clint_sales { get; set; }
        public List<preorder> Client_preorders { get; set; }
    }
}

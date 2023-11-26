using inmemory.models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inmemory
{
    internal class ConcurrentUserDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса user
        private ConcurrentDictionary<int, user> userDictionary;

        public ConcurrentUserDictionary()
        {
            userDictionary = new ConcurrentDictionary<int, user>();
        }

        // Методы для работы с ConcurrentDictionary

        // Добавление нового пользователя
        public void AddUser(user user)
        {
            userDictionary.TryAdd(user.Id, user);
        }

        // Получение пользователя по ID
        public user GetUser(int id)
        {
            user user;
            userDictionary.TryGetValue(id, out user);
            return user;
        }

        // Обновление информации о пользователе
        public void UpdateUser(user updatedUser)
        {
            userDictionary.AddOrUpdate(updatedUser.Id, updatedUser, (key, oldValue) => updatedUser);
        }

        // Удаление пользователя по ID
        public void DeleteUser(int id)
        {
            user removedUser;
            userDictionary.TryRemove(id, out removedUser);
        }

        // Получение всех пользователей
        internal IEnumerable<user> GetAllUsers()
        {
            IEnumerable<user> allUsers = (IEnumerable<user>)userDictionary.Values;

            return (IEnumerable<user>)allUsers;
        }
    }
}

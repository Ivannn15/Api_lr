using inmemory.models;
using MySqlConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace inmemory
{
    public class ConcurrentBrandDictionary
    {
        // ConcurrentDictionary для хранения экземпляров класса brand
        private readonly string connectionString;

        public ConcurrentBrandDictionary(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddBrand(brand brand)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute("INSERT INTO brands (Name, Description, Photo) VALUES (@Name, @Description, @Photo)", brand);
            }
        }

        public brand GetBrand(int id)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<brand>("SELECT * FROM brands WHERE Id = @Id", new { Id = id });
            }
        }

        public void UpdateBrand(brand updatedBrand)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute("UPDATE brands SET Name = @Name, Description = @Description, Photo = @Photo WHERE Id = @Id", updatedBrand);
            }
        }

        public void DeleteBrand(int id)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute("DELETE FROM brands WHERE Id = @Id", new { Id = id });
            }
        }

        public IEnumerable<brand> GetAllBrands()
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                return connection.Query<brand>("SELECT * FROM brands").ToList();
            }
        }
    }
}

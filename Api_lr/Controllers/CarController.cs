using Dapper;
using inmemory.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly string _connectionString;

        public CarController(MySqlConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        [HttpGet]
        public IEnumerable<car> GetAll()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<car>("SELECT * FROM cars");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<car> GetCar(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var car = connection.QueryFirstOrDefault<car>("SELECT * FROM cars WHERE Id = @Id", new { Id = id });
                if (car == null)
                {
                    return NotFound();
                }
                return car;
            }
        }

        [HttpPost]
        public ActionResult AddCar([FromBody] car car)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем максимальное значение Id из таблицы cars
                int maxId = connection.ExecuteScalar<int>("SELECT MAX(Id) FROM cars");

                // Увеличиваем значение Id на 1
                int newId = maxId + 1;

                // Вставляем новую запись с проставленным значением Id
                connection.Execute("INSERT INTO cars (Id, Name, Description, Photo, Type, brandId, modelId) VALUES (@Id, @Name, @Description, @Photo, @Type, @brandId, @modelId)",
                    new { Id = newId, car.Name, car.Description, car.Photo, car.Type, car.brandId, car.modelId });
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCar(int id, [FromBody] car updatedCar)
        {
            updatedCar.Id = id;
            if (id != updatedCar.Id)
            {
                return BadRequest();
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var existingCar = connection.QueryFirstOrDefault<car>("SELECT * FROM cars WHERE Id = @id", new { id });
                if (existingCar == null)
                {
                    return NotFound();
                }
                var queryBuilder = new StringBuilder("UPDATE cars SET ");
                var parameters = new DynamicParameters();
                if (!string.IsNullOrEmpty(updatedCar.Name) && updatedCar.Name != existingCar.Name && updatedCar.Name != "string")
                {
                    queryBuilder.Append("Name = @Name, ");
                    parameters.Add("Name", updatedCar.Name);
                }
                else
                {
                    parameters.Add("ExistingName", existingCar.Name);
                }
                if (!string.IsNullOrEmpty(updatedCar.Description) && updatedCar.Description != existingCar.Description && updatedCar.Description != "string")
                {
                    queryBuilder.Append("Description = @Description, ");
                    parameters.Add("Description", updatedCar.Description);
                }
                else
                {
                    parameters.Add("ExistingDescription", existingCar.Description);
                }
                if (!string.IsNullOrEmpty(updatedCar.Photo) && updatedCar.Photo != existingCar.Photo && updatedCar.Photo != "string")
                {
                    queryBuilder.Append("Photo = @Photo, ");
                    parameters.Add("Photo", updatedCar.Photo);
                }
                else
                {
                    parameters.Add("ExistingPhoto", existingCar.Photo);
                }
                if (!string.IsNullOrEmpty(updatedCar.Type) && updatedCar.Type != existingCar.Type && updatedCar.Type != "string")
                {
                    queryBuilder.Append("Type = @Type, ");
                    parameters.Add("Type", updatedCar.Type);
                }
                else
                {
                    parameters.Add("ExistingType", existingCar.Type);
                }
                queryBuilder.Remove(queryBuilder.Length - 2, 2); // Удаление запятой и пробела в конце запроса
                queryBuilder.Append(" WHERE Id = @Id");
                parameters.Add("Id", id);
                connection.Execute(queryBuilder.ToString(), parameters);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCar(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var existingCar = connection.QueryFirstOrDefault<car>("SELECT * FROM cars WHERE Id = @Id", new { Id = id });
                if (existingCar == null)
                {
                    return NotFound();
                }

                connection.Execute("DELETE FROM cars WHERE Id = @Id", new { Id = id });
            }
            return Ok();
        }
    }

    
}
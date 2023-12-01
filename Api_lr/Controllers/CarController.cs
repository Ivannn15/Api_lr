using Dapper;
using inmemory.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            if (id != updatedCar.Id)
            {
                return BadRequest();
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var existingCar = connection.QueryFirstOrDefault<car>("SELECT * FROM cars WHERE Id = @Id", new { Id = id });
                if (existingCar == null)
                {
                    return NotFound();
                }

                connection.Execute("UPDATE cars SET Name = @Name, Description = @Description, Photo = @Photo, Type = @Type, brandId = @brandId, modelId = @modelId WHERE Id = @Id",
                    new { updatedCar.Name, updatedCar.Description, updatedCar.Photo, updatedCar.Type, Id = id, updatedCar.brandId, updatedCar.modelId });
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api_lr.model;
using Dapper;
using inmemory.models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly string _connectionString;

        public ModelController(MySqlConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<special_model>> GetModel(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM models WHERE Id = @Id";
                var model = await connection.QuerySingleOrDefaultAsync<special_model>(query, new { Id = id });
                if (model == null)
                {
                    return NotFound();
                }
                return model;
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddModel([FromBody] special_model model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Проверка наличия указанного brandId в базе данных
                var brandExists = await connection.ExecuteScalarAsync<bool>("SELECT EXISTS(SELECT 1 FROM brands WHERE Id = @brandId)", new { brandId = model.brandId });

                if (!brandExists)
                {
                    return BadRequest("Брэнда с таким id не существует");
                }

                // Получение текущего максимального id
                var maxId = await connection.ExecuteScalarAsync<int>("SELECT MAX(Id) FROM models");

                var newId = maxId + 1;

                // Проставление нового id в модель
                model.Id = newId;

                var query = "INSERT INTO models (Id, Name, Description, Photo, brandId) VALUES (@Id, @Name, @Description, @Photo, @brandId)";
                await connection.ExecuteAsync(query, model);
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateModel(int id, [FromBody] special_model updatedModel)
        {
            updatedModel.Id = id;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var existingModel = await connection.QuerySingleOrDefaultAsync<special_model>("SELECT * FROM models WHERE Id = @Id", new { Id = id });
                if (existingModel == null)
                {
                    return NotFound();
                }

                var queryBuilder = new StringBuilder("UPDATE models SET ");
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(updatedModel.Name) && updatedModel.Name != existingModel.Name && updatedModel.Name != "string")
                {
                    queryBuilder.Append("Name = @Name, ");
                    parameters.Add("Name", updatedModel.Name);
                }
                else
                {
                    parameters.Add("Name", existingModel.Name);
                }

                if (!string.IsNullOrEmpty(updatedModel.Description) && updatedModel.Description != existingModel.Description && updatedModel.Description != "string")
                {
                    queryBuilder.Append("Description = @Description, ");
                    parameters.Add("Description", updatedModel.Description);
                }
                else
                {
                    parameters.Add("Description", existingModel.Description);
                }

                if (!string.IsNullOrEmpty(updatedModel.Photo) && updatedModel.Photo != existingModel.Photo && updatedModel.Photo != "string")
                {
                    queryBuilder.Append("Photo = @Photo, ");
                    parameters.Add("Photo", updatedModel.Photo);
                }
                else
                {
                    parameters.Add("Photo", existingModel.Photo);
                }

                if (updatedModel.brandId != 0 && updatedModel.brandId != existingModel.brandId)
                {
                    queryBuilder.Append("brandId = @brandId, ");
                    parameters.Add("brandId", updatedModel.brandId);
                }
                else
                {
                    parameters.Add("brandId", existingModel.brandId);
                }

                queryBuilder.Remove(queryBuilder.Length - 2, 2); // Удаление запятой и пробела в конце запроса
                queryBuilder.Append(" WHERE Id = @Id");
                parameters.Add("Id", id);

                await connection.ExecuteAsync(queryBuilder.ToString(), parameters);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteModel(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var existingModel = await connection.QuerySingleOrDefaultAsync<special_model>("SELECT * FROM models WHERE Id = @Id", new { Id = id });
                if (existingModel == null)
                {
                    return NotFound();
                }

                var query = "DELETE FROM models WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<special_model>> GetAll()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM models";
                var models = await connection.QueryAsync<special_model>(query);
                return models.ToList();
            }
        }
    }
}
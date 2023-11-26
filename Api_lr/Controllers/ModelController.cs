using System.Collections.Generic;
using System.Linq;
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
                var query = "INSERT INTO models (Name, Description, Photo, brandId) VALUES (@Name, @Description, @Photo, @brandId)";
                await connection.ExecuteAsync(query, model);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateModel(int id, [FromBody] special_model updatedModel)
        {
            if (id != updatedModel.Id)
            {
                return BadRequest();
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var existingModel = await connection.QuerySingleOrDefaultAsync<special_model>("SELECT * FROM models WHERE Id = @Id", new { Id = id });
                if (existingModel == null)
                {
                    return NotFound();
                }

                var query = "UPDATE models SET Name = @Name, Description = @Description, Photo = @Photo, brandId = @brandId WHERE Id = @Id";
                await connection.ExecuteAsync(query, updatedModel);
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
using inmemory.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Dapper;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly string _connectionString;

        public BrandController()
        {
            _connectionString = "server=localhost;user=root;password=root;database=main_db;";
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var brands = connection.Query<brand>("SELECT * FROM brands");
                return Ok(brands);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var brand = connection.QueryFirstOrDefault<brand>("SELECT * FROM brands WHERE Id = @Id", new { Id = id });
                if (brand == null)
                {
                    return NotFound();
                }
                return Ok(brand);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] brand brand)
        {
            if (brand == null)
            {
                return BadRequest();
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO brands (Name, Description, Photo) VALUES (@Name, @Description, @Photo)", brand);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] brand updatedBrand)
        {
            if (updatedBrand == null || updatedBrand.Id != id)
            {
                return BadRequest();
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                var brand = connection.QueryFirstOrDefault<brand>("SELECT * FROM brands WHERE Id = @Id", new { Id = id });
                if (brand == null)
                {
                    return NotFound();
                }

                updatedBrand.Id = id;
                connection.Execute("UPDATE brands SET Name = @Name, Description = @Description, Photo = @Photo WHERE Id = @Id", updatedBrand);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                var brand = connection.QueryFirstOrDefault<brand>("SELECT * FROM brands WHERE Id = @Id", new { Id = id });
                if (brand == null)
                {
                    return NotFound();
                }

                connection.Execute("DELETE FROM brands WHERE Id = @Id", new { Id = id });
            }

            return NoContent();
        }
    }
}
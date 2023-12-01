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

                foreach (var brand in brands)
                {
                    // Запрос для получения автомобилей этого бренда
                    var carsQuery = "SELECT * FROM cars WHERE brandId = @BrandId";
                    var cars = connection.Query<car>(carsQuery, new { BrandId = brand.Id });
                    brand.brands_car = cars.ToList();

                    // Запрос для получения моделей этого бренда
                    var modelsQuery = "SELECT * FROM models WHERE brandId = @BrandId";
                    var models = connection.Query<inmemory.models.model>(modelsQuery, new { BrandId = brand.Id });
                    brand.brands_model = models.ToList();

                    // Запрос для получения автомобилей для каждой модели
                    foreach (var model in brand.brands_model)
                    {
                        var modelCarsQuery = "SELECT * FROM cars WHERE brandId = @BrandId AND modelId = @ModelId";
                        var modelCars = connection.Query<car>(modelCarsQuery, new { BrandId = brand.Id, ModelId = model.Id });
                        model.models_car = modelCars.ToList();
                    }
                }

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

                // Запрос для получения автомобилей этого бренда
                var carsQuery = "SELECT * FROM cars WHERE brandId = @BrandId";
                var cars = connection.Query<car>(carsQuery, new { BrandId = brand.Id });
                brand.brands_car = cars.ToList();

                // Запрос для получения моделей этого бренда
                var modelsQuery = "SELECT * FROM models WHERE brandId = @BrandId";
                var models = connection.Query<inmemory.models.model>(modelsQuery, new { BrandId = brand.Id });
                brand.brands_model = models.ToList();

                // Запрос для получения автомобилей для каждой модели
                foreach (var model in brand.brands_model)
                {
                    var modelCarsQuery = "SELECT * FROM cars WHERE brandId = @BrandId AND modelId = @ModelId";
                    var modelCars = connection.Query<car>(modelCarsQuery, new { BrandId = brand.Id, ModelId = model.Id });
                    model.models_car = modelCars.ToList();
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
                // Получаем текущий максимальный Id из таблицы brands
                int maxId = connection.ExecuteScalar<int>("SELECT MAX(Id) FROM brands");

                // Увеличиваем значение на 1 и присваиваем новому объекту brand
                brand.Id = maxId + 1;

                // Вставляем новую запись в таблицу brands
                connection.Execute("INSERT INTO brands (Id, Name, Description, Photo) VALUES (@Id, @Name, @Description, @Photo)", brand);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] brand updatedBrand)
        {
            updatedBrand.Id = id;
            if (updatedBrand == null || updatedBrand.Id != id)
            {
                return BadRequest();
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                var brand = connection.QueryFirstOrDefault<brand>("SELECT * FROM brands WHERE Id = @id", new { Id = id });
                if (brand == null)
                {
                    return NotFound();
                }
                var updateFields = new List<string>();
                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(updatedBrand.Name) && updatedBrand.Name != brand.Name && updatedBrand.Name != "string")
                {
                    updateFields.Add("Name = @Name");
                    parameters.Add("Name", updatedBrand.Name);
                }
                else
                {
                    parameters.Add("Name", brand.Name);
                }

                if (!string.IsNullOrEmpty(updatedBrand.Description) && updatedBrand.Description != brand.Description && updatedBrand.Description != "string")
                {
                    updateFields.Add("Description = @Description");
                    parameters.Add("Description", updatedBrand.Description);
                }
                else
                {
                    parameters.Add("Description", brand.Description);
                }

                if (!string.IsNullOrEmpty(updatedBrand.Photo) && updatedBrand.Photo != brand.Photo && updatedBrand.Photo != "string" )
                {
                    updateFields.Add("Photo = @Photo");
                    parameters.Add("Photo", updatedBrand.Photo);
                }
                else
                {
                    parameters.Add("Photo", brand.Photo);
                }

                parameters.Add("Id", id);
                var updateQuery = $"UPDATE brands SET {string.Join(", ", updateFields)} WHERE Id = @Id";
                connection.Execute(updateQuery, parameters);
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
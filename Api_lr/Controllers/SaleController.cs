using inmemory.models;
using inmemory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private ConcurrentSaleDictionary saleDictionary;

        public SaleController()
        {
            saleDictionary = new ConcurrentSaleDictionary();
        }

        [HttpGet("{id}")]
        public ActionResult<sale> GetSale(int id)
        {
            var sale = saleDictionary.GetSale(id);
            if (sale == null)
            {
                return NotFound();
            }
            return sale;
        }

        [HttpPost]
        public ActionResult AddSale([FromBody] sale sale)
        {
            saleDictionary.AddSale(sale);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateSale(int id, [FromBody] sale updatedSale)
        {
            if (id != updatedSale.id)
            {
                return BadRequest();
            }

            var existingSale = saleDictionary.GetSale(id);
            if (existingSale == null)
            {
                return NotFound();
            }

            saleDictionary.UpdateSale(updatedSale);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSale(int id)
        {
            var existingSale = saleDictionary.GetSale(id);
            if (existingSale == null)
            {
                return NotFound();
            }

            saleDictionary.DeleteSale(id);
            return Ok();
        }

        [HttpGet]
        public IEnumerable<sale> GetAll()
        {
            return saleDictionary.GetAllSales();
        }
    }
}

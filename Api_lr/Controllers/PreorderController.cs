using inmemory.models;
using inmemory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreorderController : ControllerBase
    {
        public ConcurrentPreorderDictionary _preorderDictionary;

        public PreorderController(ConcurrentPreorderDictionary preorderDictionary)
        {
            _preorderDictionary = preorderDictionary;
        }

        [HttpGet("{id}")]
        public ActionResult<preorder> GetPreorder(int id)
        {
            var preorder = _preorderDictionary.GetPreorder(id);
            if (preorder == null)
            {
                return NotFound();
            }
            return preorder;
        }

        [HttpPost]
        public ActionResult AddPreorder([FromBody] preorder preorder)
        {
            if (preorder == null)
            {
                return BadRequest();
            }
            _preorderDictionary.AddPreorder(preorder);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePreorder(int id, [FromBody] preorder updatedPreorder)
        {
            if (id != updatedPreorder.id)
            {
                return BadRequest();
            }

            var existingPreorder = _preorderDictionary.GetPreorder(id);
            if (existingPreorder == null)
            {
                return NotFound();
            }

            _preorderDictionary.UpdatePreorder(updatedPreorder);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePreorder(int id)
        {
            var existingPreorder = _preorderDictionary.GetPreorder(id);
            if (existingPreorder == null)
            {
                return NotFound();
            }

            _preorderDictionary.DeletePreorder(id);
            return Ok();
        }

        [HttpGet]
        public IEnumerable<preorder> GetAll()
        {
            return _preorderDictionary.GetAllPreorders();
        }
    }
}

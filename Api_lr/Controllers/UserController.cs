using inmemory.models;
using inmemory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_lr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ConcurrentUserDictionary userDictionary;

        public UserController()
        {
            userDictionary = new ConcurrentUserDictionary();
        }

        [HttpGet("{id}")]
        public ActionResult<user> GetUser(int id)
        {
            var user = userDictionary.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public ActionResult AddUser([FromBody] user user)
        {
            userDictionary.AddUser(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] user updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            var existingUser = userDictionary.GetUser(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            userDictionary.UpdateUser(updatedUser);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var existingUser = userDictionary.GetUser(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            userDictionary.DeleteUser(id);
            return Ok();
        }

        [HttpGet]
        public IEnumerable<user> GetAll()
        {
            return userDictionary.GetAllUsers();
        }
    }
}

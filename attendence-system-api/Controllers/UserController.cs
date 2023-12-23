using Microsoft.AspNetCore.Mvc;
using UAS.Business;
using UAS.Dependancies.Business;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_attendance_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsers _users;
        public UserController(IUsers users)
        {
            _users = users;
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("validateUser")]
        public bool Post([FromBody] dynamic body)
        {
            return _users.validateUser("1043", "051050054053");

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

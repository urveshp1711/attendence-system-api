using api_attendance_system.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UAS.Dependancies.Business;
using UAS.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_attendance_system.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsers _users;
        private readonly IJwtHandler _jwtHandler;
        public AuthController(IUsers users, IJwtHandler jwtHandler)
        {
            _users = users;
            _jwtHandler = jwtHandler;
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
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
        [AllowAnonymous]
        [Route("login")]
        public RS_UserLogin Post([FromBody] RQ_UserLogin request)
        {
            bool isValidLogin = _users.validateUser(request.userCode, request.password);
            RS_UserLogin res = new RS_UserLogin();

            if (isValidLogin)
            {
                string token = _jwtHandler.GenerateJWT(request.userCode,"urveshp.1711.purohit@gmail.com", "Admin");
                res.jwtToken = token;
                res.userCode = request.userCode;
                res.isSuccess = true;
                return res;
            }
            else
            {
                res.jwtToken = string.Empty;
                res.userCode = request.userCode;
                res.isSuccess= false;
                return res;
            }

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

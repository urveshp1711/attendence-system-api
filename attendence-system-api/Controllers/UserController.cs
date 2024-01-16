using api_attendance_system.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAS.Dependancies.Business;
using UAS.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_attendance_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        //uas_getUserInfo

        private readonly IUsers _users;
        //private readonly IJwtHandler _jwtHandler;
        public UserController(IUsers users)
        {
            _users = users;
        }

        [HttpGet]
        public IEnumerable<RS_UserInfo> Get()
        {
            return _users.getAllUsers();
        }

        // GET api/<UserController>/5

        [HttpGet("{userCode}")]
        public RS_UserInfo? Get(string userCode)
        {
            return _users.getUserInfo(userCode);
        }

        [HttpGet("getAttendanceSummary/{userCode}")]
        public IEnumerable<RS_UserAttendanceSummary>? getAttendanceSummary(string userCode)
        {
            return _users.getAttendanceSummary(userCode);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)

        {
        }

        // POST api/<UserController>
        [HttpPost]
        [Route("updateUserProfile")]
        public bool updateUserProfile([FromBody] RQ_UserProfile value)
        {
            _users.updateUserInfo(value);
            return true;
        }

        [HttpPost]
        [Route("userAttendance")]
        public RS_UserAttendance userAttendance([FromBody] RQ_UserAttendance value)
        {
            return _users.doUserAttendance(value);
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

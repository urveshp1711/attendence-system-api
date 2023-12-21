using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_attendance_system.Controllers
{
    public class UserInfo
    {
        public string? usercode { get; set; }
        public string? password { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        const string connectionString = "Server=localhost;Database=urja_system;Uid=root;Pwd=admin123;";

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
        public void Post([FromBody] UserInfo body)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("validateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usercode", body.usercode);
                    cmd.Parameters.AddWithValue("@pass", body.password);

                    connection.Open();
                    var res = cmd.ExecuteScalar();
                }
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

        private bool IsAvailable(MySqlConnection connection)
        {
            var result = false;

            try
            {
                if (connection != null)
                {
                    result = connection.Ping();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ping exception: " + e.Message);
            }

            if (connection != null)
                return result && connection.State == System.Data.ConnectionState.Open;
            else
                return false;
        }
    }
}

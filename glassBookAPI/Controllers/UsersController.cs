using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace glassBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            List<User> users = new List<User>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM user";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int user_id = reader.GetInt32(0);
                            string user_name = reader.GetString(1);
                            string password = reader.GetString(2);
                            string country = reader.GetString(3);
                            int age = reader.GetInt32(4);
                            User user = new User();
                            user.User_name = user_name;
                            user.User_id= user_id;
                            user.Password = password;
                            user.Country = country;
                            user.age = age;
                            users.Add(user);
                            // retrieve data for other columns as needed

                        }
                    }
                }
                connection.Close();
            }
            return users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult GetUser(string id)
        {
            User user = new User();
            bool b = false;
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("SELECT * FROM user WHERE user_name = '{0}'", id);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            b = true;
                            int user_id = reader.GetInt32(0);
                            string user_name = reader.GetString(1);
                            string password = reader.GetString(2);
                            string country = reader.GetString(3);
                            int age = reader.GetInt32(4);
                            user.User_name = user_name;
                            user.User_id = user_id;
                            user.Password = password;
                            user.Country = country;
                            user.age = age;

                        }
                    }
                }
                connection.Close();
            }
            if (!b)
            {
                return NotFound();
            }
            return Ok(user);
        }


        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

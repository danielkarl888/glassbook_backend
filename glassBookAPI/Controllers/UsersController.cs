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
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("select u.user_name,age,country,avg(rate) as avg_rate ,count(comment_id) as numComments" +
                    " from user as u, comment as c " +
                    "where u.user_name = '{0}' and u.user_name = c.user_name", id);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int j = 1;
                        while (reader.Read())
                        {
                            b = true;
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            row.Add("seq", j++);
                            rows.Add(row);
                            // retrieve data for other columns as needed
                        }
                    }
                    connection.Close();
                }
                if (!b)
                {
                    return NotFound();
                }
                return Ok(rows);
            }
        }

        [HttpGet("Login/")]
        public IActionResult LoginContact(string user_name, string password)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("SELECT * FROM glassbook.user as u " +
                    "WHERE u.user_name = '{0}' AND u.password = '{1}'", user_name, password);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int j = 1;
                        while (reader.Read())
                        {
                            b = true;
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            row.Add("seq", j++);
                            rows.Add(row);

                        }
                    }
                    connection.Close();
                }
                if (!b)
                {
                    return NotFound();
                }
                return Ok(rows);
            }

        }


        [HttpGet("isExistUserName/")]
        public IActionResult isExistUserName(string user_name)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("SELECT * FROM glassbook.user as u " +
                    "WHERE u.user_name = '{0}'", user_name);

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int j = 1;
                        while (reader.Read())
                        {
                            b = true;
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            row.Add("seq", j++);
                            rows.Add(row);

                        }
                    }
                    connection.Close();
                }
                if (!b)
                {
                    return NotFound();
                }
                return Ok(rows);
            }

        }
        [HttpGet("Register/")]
        public IActionResult Register(string user_name, string password, string country, string age)
        {
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("INSERT INTO user (user_name, password, country, age) VALUES ('{0}', '{1}','{2}', {3})",
                    user_name, password, country, age);
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataReader reader = command.ExecuteReader();

                return Ok();
            }

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

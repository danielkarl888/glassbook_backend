using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

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
            try
            {

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
                                user.User_id = user_id;
                                user.Password = password;
                                user.Country = country;
                                user.Age = age;
                                users.Add(user);
                                // retrieve data for other columns as needed

                            }
                        }
                    }
                    connection.Close();
                }
                return users;
            }
            catch (Exception ex)
            {
                User user = new User();
                user.User_name = "error with server";
                users.Add(user);
                return users;

            }
        }
        // GET a user's details
        [HttpGet("{id}")]
        public ActionResult GetUser(string id)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            try
            {
                string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Format("select u.user_name,age,country," +
                        " COALESCE(avg(rate),0) as Avg_rate,COALESCE(count(comment_id),0) as numComments" +
                        " from user u left join comment as c  " +
                        " ON c.user_name = u.user_name" +
                        " where u.user_name = '{0}' GROUP BY u.user_name, u.age, u.country;", id);

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
            } catch (Exception ex)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("age", "Error - Please refresh the page");
                row.Add("country", "Error - Please refresh the page");
                row.Add("numComments", "Error - Please refresh the page");

                rows.Add(row);
                return Ok(rows);
            }
        }

        [HttpGet("Login/")]
        public IActionResult LoginContact(string user_name, string password)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            try
            {

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
            catch (Exception ex)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("seq", "Error - Please refresh the page");
                rows.Add(row);
                return BadRequest(rows);
            }

        }


        [HttpGet("isExistUserName/")]
        public bool isExistUserName(string user_name)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            try
            {


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
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [HttpPost("Register/")]
        public IActionResult Register([Bind("Username,Password,Country, Age")] User user)
        {
            try
            {
                if (!isExistUserName(user.User_name) && user.Age > 0 && user.Age <= 120 && Regex.IsMatch(user.Password, @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]*$")&& !user.Country.Equals("") && !user.User_name.Equals(""))
                {
                    user.Age = (int)user.Age;
                    string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = string.Format("INSERT INTO user (user_name, password, country, age) VALUES ('{0}', '{1}','{2}', {3})",
                            user.User_name, user.Password, user.Country, user.Age);
                        MySqlCommand command = new MySqlCommand(sql, connection);
                        MySqlDataReader reader = command.ExecuteReader();

                        return Ok(user);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}

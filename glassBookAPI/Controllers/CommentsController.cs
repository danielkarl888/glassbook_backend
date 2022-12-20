using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace glassBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // GET: api/<CommentsController>
        [HttpGet]
        public IEnumerable<Dictionary<string, object>> Get()
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT b.book_name ,c.user_name, c.rate, c.comment_txt, c.date" +
                    "  FROM comment AS c, book AS b" +
                    "  WHERE c.book_id = b.book_id" +
                    "  ORDER BY date DESC limit 50";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int j = 1;
                        while (reader.Read())
                        {
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
                }
                connection.Close();
            }
            return rows;
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CommentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

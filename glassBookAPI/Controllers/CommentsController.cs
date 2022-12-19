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
        public IEnumerable<Comment> Get()
        {
            List<Comment> comments = new List<Comment>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=password;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM comment limit 500";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int comment_id = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);
                            string user_name = reader.GetString(2);
                            string comment_txt = reader.GetString(3);
                            int rate = reader.GetInt32(4);
                            int book_id = reader.GetInt32(5);

                            Comment c = new Comment();
                            c.Comment_Id = comment_id;
                            c.Date = date;
                            c.User_name = user_name;
                            c.Comment_txt = comment_txt;
                            c.Rate = rate;
                            c.Book_id = book_id;
                            comments.Add(c);
                            // retrieve data for other columns as needed

                        }
                    }
                }
                connection.Close();
            }
            return comments;
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

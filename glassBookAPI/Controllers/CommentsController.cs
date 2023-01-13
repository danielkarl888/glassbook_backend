using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace glassBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // GET the last 20 comments
        [HttpGet]
        public IEnumerable<Dictionary<string, object>> GetLast20()
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT b.book_id, b.book_name ,c.user_name, c.rate, c.comment_txt, c.date" +
                        "  FROM comment AS c, book AS b" +
                        "  WHERE c.book_id = b.book_id" +
                        "  ORDER BY date DESC limit 20";
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
            catch (Exception ex)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("seq", "Error occured - Please refresh the page");
                rows.Add(row);
                return rows;
            }
        }

        // GET the Top 10 averaged books
        [HttpGet("top10")]
        public IEnumerable<Dictionary<string, object>> GetTop10Books()
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {

                string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT b.book_id, b.book_name, a.author_name, b.publisher, b.img, AVG(c.rate) as avg_rate" +
                        " FROM book b JOIN comment c" +
                        " ON b.book_id = c.book_id" +
                        " JOIN author a ON b.author_id = a.author_id" +
                        " GROUP BY b.book_id" +
                        " ORDER BY avg_rate DESC LIMIT 10";
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
            catch (Exception ex)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("seq", "Error occured - Please refresh the page");
                rows.Add(row);
                return rows;
            }

        }

        // GET the last comments of a specific user
        [HttpGet("mycomments/{user_name}")]
        public ActionResult GetUser(string user_name)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            bool b = false;
            try
            {


                string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Format("SELECT c.comment_txt AS comment, c.rate, b.book_name AS book_name, a.author_name AS author_name, c.date AS date, b.book_id" +
                                            " FROM comment c INNER JOIN user u ON c.user_name = u.user_name" +
                                            " INNER JOIN book b ON c.book_id = b.book_id " +
                                            " INNER JOIN author a ON b.author_id = a.author_id " +
                                            " WHERE u.user_name = '{0}' " +
                                            " ORDER BY c.date desc limit 50", user_name);

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
            catch (Exception ex)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                row.Add("seq", "Error occured - Please refresh the page");
                rows.Add(row);
                return Ok(rows);
            }
        }

        // POST - add a comment that the user name wrote for the book_id.
        [HttpPost("addComment")]
        public ActionResult AddComment([Bind("User_name,Comment_txt,Rate, Book_id")] Comment comment)
        {
            try
            {

                string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Format("INSERT INTO comment (date, user_name, comment_txt, rate, book_id) SELECT CAST(NOW() AS DATE), '{0}', '{1}', {2}, {3}; ",
                                               comment.User_name, comment.Comment_txt, comment.Rate, comment.Book_id);

                    MySqlCommand command = new MySqlCommand(sql, connection);

                    MySqlDataReader reader = command.ExecuteReader();

                    return Ok();
                }
            } catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}

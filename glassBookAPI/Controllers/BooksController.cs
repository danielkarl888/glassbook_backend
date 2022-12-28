using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace glassBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/<BooksController>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            List<Book> books = new List<Book>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM book limit 500";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int book_id = reader.GetInt32(0);
                            string book_name = reader.GetString(1);
                            int author_id = reader.GetInt32(2);
                            string publisher = reader.GetString(3);
                            string img = reader.GetString(4);
                            string cat = reader.GetString(5);

                            Book book = new Book();
                            book.Book_id = book_id;
                            book.Book_name = book_name;
                            book.Author_id = author_id;
                            book.Publisher = publisher;
                            book.Img = img;
                            book.Category = cat;
                            books.Add(book);
                            // retrieve data for other columns as needed

                        }
                    }
                }
                connection.Close();
            }
            return books;
        }


        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult GetBook(string id)
        {
            bool b = false;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("SELECT   b.book_id, b.book_name,b.img, a.author_name,c.comment_txt, c.rate, b.publisher,c.date, c.user_name" +
                    " FROM comment c JOIN book b" +
                    " ON c.book_id = b.book_id" +
                    " JOIN author a ON b.author_id = a.author_id" +
                    " WHERE b.book_id = {0}" +
                    " ORDER BY c.date DESC LIMIT 20", id);

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
                }
                connection.Close();
            }
            if (!b)
            {
                return NotFound();
            }
            return Ok(rows);
        }

        // POST api/<BooksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

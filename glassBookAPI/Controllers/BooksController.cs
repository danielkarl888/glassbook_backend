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

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=password;";
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


        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

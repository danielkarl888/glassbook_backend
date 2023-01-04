using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics.Metrics;

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
        [HttpGet("avg/{id}")]
        public ActionResult GetBookAVG(string id)
        {
            bool b = false;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("select book_name,avg(rate) as avg_rate" +
                    " From book as b, comment as c" +
                    " where c.book_id = b.book_id and b.book_id = {0}", id);

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


        [HttpGet("SearchText/{search}")]
        public IEnumerable<Search_Book> GetByText(string search)
        {
            List<Search_Book> books = new List<Search_Book>();
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("SELECT b.book_id as id, b.book_name AS Title, a.author_name as Author,avg(c.rate) as Avg_rate, b.img as Image, b.publisher as Publisher " +
                    "from book as b " +
                    "INNER JOIN  comment as c ON c.book_id = b.book_id " +
                    "INNER JOIN  author as a  ON b.author_id = a.author_id " +
                    "WHERE a.author_name LIKE '%{0}%' OR b.book_name LIKE '%{0}%' " +
                    "group by b.book_name " +
                    "ORDER BY Avg_rate desc " +
                    "LIMIT 50;", search);
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int book_id = reader.GetInt32(0);
                            string book_name = reader.GetString(1);
                            string author_name = reader.GetString(2);
                            int avg_rate = reader.GetInt32(3);
                            string img = reader.GetString(4);
                            string publisher = reader.GetString(5);

                            Search_Book book = new Search_Book();
                            book.Book_id = book_id;
                            book.Book_name = book_name;
                            book.Author_name = author_name;
                            book.Publisher = publisher;
                            book.Img = img;
                            book.Avg_rate = avg_rate;
                            books.Add(book);

                        }
                    }
                }
                connection.Close();
            }
            return books;
        }

        [HttpGet("SearchCountry/{search}")]
        public IEnumerable<Search_Book> GetByCountry(string search)
        {
            List<Search_Book> books = new List<Search_Book>();
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("select b.book_id as id, b.book_name as Title ,a.author_name as author, avg(rate) as Avg_rating, b.img as Image, b.publisher as publisher " +
                    "From book as b, comment as c, user as u, author as a " +
                    "where c.book_id = b.book_id and u.user_name = c.user_name and u.country = '{0}' and b.author_id = a.author_id " +
                    "group by book_name " +
                    "order by Avg_rating desc " +
                    "LIMIT 10;", search);
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int book_id = reader.GetInt32(0);
                            string book_name = reader.GetString(1);
                            string author_name = reader.GetString(2);
                            int avg_rate = reader.GetInt32(3);
                            string img = reader.GetString(4);
                            string publisher = reader.GetString(5);

                            Search_Book book = new Search_Book();
                            book.Book_id = book_id;
                            book.Book_name = book_name;
                            book.Author_name = author_name;
                            book.Publisher = publisher;
                            book.Img = img;
                            book.Avg_rate = avg_rate;
                            books.Add(book);

                        }
                    }
                }
                connection.Close();
            }
            return books;
        }

        [HttpGet("SearchCategory/{search}")]
        public IEnumerable<Search_Book> GetByCategory(string search)
        {
            List<Search_Book> books = new List<Search_Book>();
            string connectionString = "server=localhost;database=glassBook;uid=root;pwd=Karl5965;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format("select b.book_id as id, b.book_name as Title ,a.author_name as author, avg(rate) as Avg_rating, b.img as Image, b.publisher as publisher " +
                    "From book as b, comment as c, user as u, author as a " +
                    "where c.book_id = b.book_id and b.category = '{0}' and u.user_name = c.user_name and b.author_id = a.author_id " +
                    "group by book_name " +
                    "order by Avg_rating desc " +
                    "LIMIT 10;", search);
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int book_id = reader.GetInt32(0);
                            string book_name = reader.GetString(1);
                            string author_name = reader.GetString(2);
                            int avg_rate = reader.GetInt32(3);
                            string img = reader.GetString(4);
                            string publisher = reader.GetString(5);

                            Search_Book book = new Search_Book();
                            book.Book_id = book_id;
                            book.Book_name = book_name;
                            book.Author_name = author_name;
                            book.Publisher = publisher;
                            book.Img = img;
                            book.Avg_rate = avg_rate;
                            books.Add(book);

                        }
                    }
                }
                connection.Close();
            }
            return books;
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

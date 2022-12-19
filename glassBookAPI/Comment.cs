namespace glassBookAPI
{
    public class Comment
    {
        public int Comment_Id { get; set; }

        public DateTime Date { get; set; }

        public string User_name { get; set; }

        public string Comment_txt { get; set; }

        public int Rate { get; set; }

        public int Book_id { get; set; }

    }
}

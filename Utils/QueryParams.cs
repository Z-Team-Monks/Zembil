namespace Zembil.Utils
{
    public class QueryParams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }
        public int Pagination { get; set; }

    }

    public class UserCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
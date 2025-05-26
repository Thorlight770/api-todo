namespace api.todo.Utils
{
    public static class DBConnection
    {
        public static string GetConnectionString(Dictionary<string, string> dictionaryString, string database)
        {
            return string.Format("Server={0},{1};" +
                "Database={2};" +
                "User id={3};" +
                "password={4};TrustServerCertificate=True", dictionaryString["Server"], dictionaryString["Port"], database, dictionaryString["Username"], dictionaryString["Password"]);
        }
    }
}

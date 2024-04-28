namespace api.todo.Utils
{
    public static class DBConnection
    {
        public static string GetConnectionString(Dictionary<string, string> dictionaryString)
        {
            return string.Format("Server={0},{1}; " +
                "Database={2}; " +
                "User id={3}; " +
                "password={4}; " +
                "Trusted_Connection=True; " +
                "MultipleActiveResultSets=true; " +
                "Encrypt=true; TrustServerCertificate=True", dictionaryString["Server"], dictionaryString["Port"], dictionaryString["Database"], dictionaryString["Username"], dictionaryString["Password"]);
        }
    }
}

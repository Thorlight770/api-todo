using StackExchange.Redis;

namespace api.todo.Utils
{
    public static class RedisConnection
    {
        private static readonly ConfigurationBuilder _config;

        public static Dictionary<string, string> GetConnectionDictionary(string serverName, string endPoint)
        {
            var response = new Dictionary<string, string>();
            try
            {
                if (serverName == "Redis")
                {
                    ConfigurationOptions option = new ConfigurationOptions
                    {
                        AbortOnConnectFail = true,
                        EndPoints = { endPoint }
                    };

                    var redisConnection = ConnectionMultiplexer.Connect(option);
                    var redisCache = redisConnection.GetDatabase();

                    if (redisConnection.IsConnected)
                    {
                        var port = redisCache.StringGet("port");
                        var user = redisCache.StringGet("username");
                        var password = redisCache.StringGet("password");
                        var database = redisCache.StringGet("database");

                        response.Add("Server", redisCache.StringGet("server").ToString());
                        response.Add("Port", redisCache.StringGet("port").ToString());
                        response.Add("Database", redisCache.StringGet("database").ToString());
                        response.Add("Username", redisCache.StringGet("username").ToString());
                        response.Add("Password", redisCache.StringGet("password").ToString());
                    }
                    else throw new Exception("Redis Connection Failed !");
                }
                else throw new Exception("Server Name Not Found !");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}

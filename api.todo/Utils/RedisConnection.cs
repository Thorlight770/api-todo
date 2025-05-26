using StackExchange.Redis;

namespace api.todo.Utils
{
    public static class RedisConnection
    {
        private static readonly ConfigurationBuilder _config;

        public static Dictionary<string, string> GetConnectionDictionary(string serverName, string endPoint, string epvID)
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
                        var port = redisCache.StringGet(epvID + "-" + "port");
                        var user = redisCache.StringGet(epvID + "-" + "username");
                        var password = redisCache.StringGet(epvID + "-" + "password");
                        var server = redisCache.StringGet(epvID + "-" + "server");

                        response.Add("Port", port);
                        response.Add("Username", user);
                        response.Add("Password", password);
                        response.Add("Server", server);
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

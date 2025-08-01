using api.todo.Model;
using Commond_Lib.Models;
using Commond_Lib.RestClient;

namespace api.todo.RestClient.Impl
{
    public class TransactionRestClient : ITransactionRestClient
    {
        private readonly HttpClient _httpClient;
        public TransactionRestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SvcModels<Transaction>> InquiryTransactionByIDRestClient(string id)
        {
            return await ClsRestClient.PostRestClient<string, SvcModels<Transaction>>(_httpClient, "", id);
        }
    }
}

using api.todo.Model;
using Commond_Lib.Models;

namespace api.todo.RestClient
{
    public interface ITransactionRestClient
    {
        Task<SvcModels<Transaction>> InquiryTransactionByIDRestClient(string id);
    }
}

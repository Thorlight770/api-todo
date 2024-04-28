using api.todo.Model.Enum;

namespace api.todo.Model
{
    public class ApiModel<T>
    {
        public T? Data { get; set; }
        public int? PageIndex { get; set; }
        public int? RowPerPage { get; set; }
        public int? TotalPage { get; set; }
        public List<AdditionalMessage>? Messages { get; set; }
        public string? LogReff { get; set; }

        public ApiModel()
        {
            if (string.IsNullOrEmpty(LogReff)) LogReff = Guid.NewGuid().ToString();
            Messages = new List<AdditionalMessage>();
        }
    }

    public class AdditionalMessage
    {
        public string? HTTPResponse { get; set; }
        public MessageType MessageType { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }

        public AdditionalMessage() 
        { 
        }

        public AdditionalMessage(string? httpResponse, MessageType messageType, string? description, string? source)
        {
            HTTPResponse = httpResponse;
            MessageType = messageType;
            Description = description;
            Source = source;
        }
    }
}

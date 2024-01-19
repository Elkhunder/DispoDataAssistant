namespace DispoDataAssistant.Data.Models
{
    public class ResultObject
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        public ResultObject(bool isSuccessful, string message, object? data = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Data = data;
        }
    }
}

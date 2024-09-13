
namespace TesteDigitas.Domain.Models
{
    public class DefaultApiResponse
    {

        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
        public string Notification { get; set; }


        public DefaultApiResponse(object response = null, string notification = "", bool isSuccessful = true)
        {
            IsSuccessful = response != null && isSuccessful;
            Data = response;
            Notification = notification;
        }

    }
}

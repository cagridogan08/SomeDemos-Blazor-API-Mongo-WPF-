using System.Text.Json.Serialization;

namespace NotificationAPI.Models
{
    public class ResponseModel
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }
            [JsonPropertyName("body")]
            public string Body { get; set; }
        }
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "high";
        [JsonPropertyName("data")]
        public DataPayload Data { get; set; }
        [JsonPropertyName("notification")]
        public DataPayload Notification { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace NotificationAPI.Models
{
    public class NotificationModel
    {
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        [JsonPropertyName("isAndroiodDevice")]
        public bool IsAndroid { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}

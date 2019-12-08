using System.Text.Json.Serialization;

namespace GearboxService.Models
{
    public class BearerToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
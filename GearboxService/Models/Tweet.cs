using System.Text.Json.Serialization;

namespace GearboxService.Models
{
    public class Tweet
    {
        [JsonPropertyName("full_text")]
        public string FullText { get; set; }
    }
}
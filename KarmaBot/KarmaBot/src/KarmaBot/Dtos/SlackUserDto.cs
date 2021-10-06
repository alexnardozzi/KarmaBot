using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace KarmaBot.Dtos
{
    public class SlackUserDto
    {
        [JsonPropertyName("id")]
        public string UserId { get; set; }
        [JsonPropertyName("real_name")]
        public string RealName { get; set; }
        
        public SlackUserDto(){}
    }
}
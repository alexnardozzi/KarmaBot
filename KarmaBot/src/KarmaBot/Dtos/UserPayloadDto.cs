using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace KarmaBot.Dtos
{
    public class UserPayloadDto
    {
        [JsonPropertyName("user")]
        public SlackUserDto User { get; set; }
        
        public UserPayloadDto(){}
    }
}
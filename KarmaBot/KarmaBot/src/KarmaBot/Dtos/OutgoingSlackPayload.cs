using Newtonsoft.Json;

namespace KarmaBot.Dtos
{
    public class OutgoingSlackPayload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
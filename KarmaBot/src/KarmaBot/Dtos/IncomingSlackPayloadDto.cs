namespace KarmaBot.Dtos
{
    public class IncomingSlackPayloadDto
    {
        public string Token { get; set; }
        public string Challenge { get; set; }
        public string Type { get; set; }
        public PayloadEventDto Event { get; set; }

        public IncomingSlackPayloadDto(){}
    }
}
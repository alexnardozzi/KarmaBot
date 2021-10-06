using Microsoft.Extensions.Configuration;

namespace KarmaBot
{
    public class SlackConnectionInformation
    {
        public string Bearer { get; set; }

        public SlackConnectionInformation(IConfiguration configuration)
        {
            Bearer = configuration.GetSection("SlackConnectionInformation")["BearerToken"];
        }
    }
}
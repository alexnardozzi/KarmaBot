using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using KarmaBot.Dtos;
using KarmaBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KarmaBot.Controllers
{
    [Route("api/slack/")]
    [ApiController]
    public class SlackController : Controller
    {
        private readonly IKarmaService _karmaService;

        public SlackController(IKarmaService karmaService)
        {
            _karmaService = karmaService;
        }
        
        [HttpPost("entry")]
        public async Task<ActionResult<string>> SlackVerification([FromBody] IncomingSlackPayloadDto payload)
        {
            var headers = Request.Headers;
            Response.Headers.Add("X-Slack-No-Retry", "1");
            LambdaLogger.Log($"--- New Event: \n{JsonConvert.SerializeObject(payload)} \nwith headers: {JsonConvert.SerializeObject(headers)}");

            if (payload.Type == "url_verification")
            {
                LambdaLogger.Log($"--- Responding to Slack verification challenge with {payload.Challenge}");
                return Ok(payload.Challenge);
            }

            if (!IsValidSlackEvent(payload, headers))
            {
                return Ok();
            }

            await _karmaService.UpdateKarma(payload);

            return Ok();
        }

        private static bool IsValidSlackEvent(IncomingSlackPayloadDto payload, IHeaderDictionary headers)
        {
            var text = payload.Event.Text;

            // Make sure text starts with a user mention
            if (!(text.StartsWith("<@") || text.StartsWith("@")))
            {
                LambdaLogger.Log("--- Ignoring non user-mentioning message");
                return false;
            }

            // Make sure text ends with incrementing or decrementing symbols
            if (!text.EndsWith("++") && !text.EndsWith("--"))
            {
                LambdaLogger.Log("--- Ignoring non-karmic mention");
                return false;
            }
            
            // Ignore retries
            if (headers.Keys.Contains("X-Slack-Retry-Num"))
            {
                LambdaLogger.Log("Ignoring retries");
                return false;
            }
            
            // Make sure message wasn't sent by KarmaBot
            if (payload.Event.User == "U02GN7YKDGA")
            {
                LambdaLogger.Log("--- Ignoring self message");
                return false;
            }
            
            // Make sure message follows the correct format eg: "@User ++"
            var delimiterIndex = text.Contains('>') ? text.IndexOf('>') + 1 : text.IndexOf(' ');
            if (text.Substring(delimiterIndex).TrimStart().Distinct().Count() != 1)
            {
                LambdaLogger.Log("--- Improperly formed karma command");
                return false;
            }
            
            return true;
        }
    }
}
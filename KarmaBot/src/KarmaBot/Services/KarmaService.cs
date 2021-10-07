using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using KarmaBot.Dtos;
using KarmaBot.Models;
using KarmaBot.Repositories;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace KarmaBot.Services
{
    public class KarmaService : IKarmaService
    {
        private readonly IKarmaRepository _karmaRepository;
        private readonly SlackConnectionInformation _slackInfo;
        private const long CHANGE_LIMIT = 5;

        public KarmaService(IKarmaRepository karmaRepository, SlackConnectionInformation slackInfo)
        {
            _karmaRepository = karmaRepository;
            _slackInfo = slackInfo;
        }
        
        public async Task<bool> UpdateKarma(IncomingSlackPayloadDto payload)
        {
            var text = payload.Event.Text;
            var isNonUser = text[0].Equals('@');
            
            var targetUserId = isNonUser ? text.Substring(1, text.IndexOf(' ') - 1) : text.Substring(2, text.IndexOf('>') - 2);
            var karmaGiverId = payload.Event.User;

            if (targetUserId.Equals(karmaGiverId))
            {
                LambdaLogger.Log($"--- User {karmaGiverId} tried to give karma to themself");
                var punishment = await _karmaRepository.UpdateKarma(karmaGiverId, -1, isNonUser);
                var punishMessage = await PostSlackMessage(payload.Event.Channel, $"<@{targetUserId}> tried to give themself karma. Their karma has been reduced to {punishment.KarmaCount}");
                return punishMessage;
            }

            var karmaChange = DetermineKarmaChange(text);
            if (karmaChange > CHANGE_LIMIT || karmaChange < -CHANGE_LIMIT)
            {
                LambdaLogger.Log($"--- Karma Change of {karmaChange} exceeded limit of {CHANGE_LIMIT}");
                var limitMessage = await PostSlackMessage(payload.Event.Channel, $"Karma change of {karmaChange} exceeded limit of {CHANGE_LIMIT}");
                return limitMessage;
            }
            
            LambdaLogger.Log($"--- Targeted User: {targetUserId}\nKarma Change: {karmaChange}");

            var updatedKarma = await _karmaRepository.UpdateKarma(targetUserId, karmaChange, isNonUser);
            await _karmaRepository.UpdateKarmaStats(karmaGiverId, karmaChange);

            var userName = updatedKarma.User.Name;
            if (string.IsNullOrEmpty(userName))
            {
                var userDto = await UpdateUser(targetUserId);
                userName = userDto?.RealName ?? $"<@{targetUserId}>";
            }

            var changeResult = await PostSlackMessage(payload.Event.Channel, $"{userName}'s karma is now {updatedKarma.KarmaCount}");
            
            return changeResult;
        }

        private async Task<SlackUserDto> UpdateUser(string slackUserId)
        {
            var client = GetClient();

            var result = await client.GetAsync($"https://slack.com/api/users.info?user={slackUserId}");
            var content = await JsonSerializer.DeserializeAsync<UserPayloadDto>(await result.Content.ReadAsStreamAsync());
            LambdaLogger.Log($"--- Got response: \n{await result.Content.ReadAsStringAsync()}");

            var userDto = content.User;
            
            LambdaLogger.Log($"--- Updating user {userDto.UserId} with name {userDto.RealName}");

            var user = new User
            {
                SlackUserId = userDto.UserId,
                Name = userDto.RealName
            };

            await _karmaRepository.UpdateUser(user);

            return userDto;
        }

        private long DetermineKarmaChange(string message)
        {
            var delimiterIndex = message.Contains('>') ? message.IndexOf('>') + 1 : message.IndexOf(' ');
            var text = message.Substring(delimiterIndex);
            var delta = text.Trim().Length - 1;
            return text.Contains('+') ? delta : delta * -1;
        }

        private async Task<bool> PostSlackMessage(string channel, string text)
        {
            var client = GetClient();

            var outgoingPayload = new OutgoingSlackPayload()
            {
                Channel = channel,
                Text = text
            };
            
            LambdaLogger.Log($"--- Outgoing request body: \n{JsonConvert.SerializeObject(outgoingPayload)}");

            var response = await client.PostAsync("https://slack.com/api/chat.postMessage",
                new StringContent(JsonConvert.SerializeObject(outgoingPayload), Encoding.Default, "application/json"));
            
            LambdaLogger.Log($"--- Response was: \n{JsonConvert.SerializeObject(response)}");

            return response.IsSuccessStatusCode;
        }

        private HttpClient GetClient()
        {
            return new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", _slackInfo.Bearer)
                }
            };
        }
    }
}
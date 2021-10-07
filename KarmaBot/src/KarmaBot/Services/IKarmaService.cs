using System.Threading.Tasks;
using KarmaBot.Dtos;

namespace KarmaBot.Services
{
    public interface IKarmaService
    {
        Task<bool> UpdateKarma(IncomingSlackPayloadDto payload);
    }
}
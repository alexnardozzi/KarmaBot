using System.Threading.Tasks;
using KarmaBot.Models;

namespace KarmaBot.Repositories
{
    public interface IKarmaRepository
    {
        Task<Karma> GetKarma(string slackUserId);
        Task<Karma> UpdateKarma(string slackUserId, long karmaChange);
        Task<Karma> UpdateKarmaStats(string slackUserId, long karmaChange);
    }
}
using System.Threading.Tasks;
using KarmaBot.Models;

namespace KarmaBot.Repositories
{
    public interface IKarmaRepository
    {
        Task<Karma> GetKarma(string slackUserId, bool isNonUser);
        Task<Karma> UpdateKarma(string slackUserId, long karmaChange, bool isNonUser);
        Task<Karma> UpdateKarmaStats(string slackUserId, long karmaChange);
        Task<User> UpdateUser(User user);
    }
}
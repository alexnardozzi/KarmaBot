using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using KarmaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace KarmaBot.Repositories
{
    public class KarmaRepository : IKarmaRepository
    {
        private readonly KarmaContext _context;

        public KarmaRepository(KarmaContext context)
        {
            _context = context;
        }

        public async Task<Karma> GetKarma(string slackUserId)
        {
            var user = await _context.Users
                .Include(u => u.Karma)
                .SingleOrDefaultAsync(u => u.SlackUserId == slackUserId);
            
            if (user == null)
            {
                LambdaLogger.Log($"Creating new user for slack ID: {slackUserId}");
                user = await CreateUser(slackUserId);
            }

            return user.Karma;
        }

        public async Task<Karma> UpdateKarma(string slackUserId, long karmaChange)
        {
            var currentKarma = await GetKarma(slackUserId);
            var updatedKarmaCount = currentKarma.KarmaCount + karmaChange;

            currentKarma.KarmaCount = updatedKarmaCount;
            if (currentKarma.HighestKarma <= updatedKarmaCount)
            {
                currentKarma.HighestKarma = updatedKarmaCount;
                currentKarma.HighestKarmaDate = DateTime.Now;
            }

            if (currentKarma.LowestKarma >= updatedKarmaCount)
            {
                currentKarma.LowestKarma = updatedKarmaCount;
                currentKarma.LowestKarmaDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            
            return currentKarma;
        }

        public async Task<Karma> UpdateKarmaStats(string slackUserId, long karmaChange)
        {
            var currentKarma = await GetKarma(slackUserId);
            if (karmaChange > 0)
            {
                currentKarma.PositiveKarmaGiven += karmaChange;
            }
            else
            {
                currentKarma.NegativeKarmaGiven += karmaChange * -1;
            }

            await _context.SaveChangesAsync();
            return currentKarma;
        }

        public async Task<User> UpdateUser(User user)
        {
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.SlackUserId == user.SlackUserId);
            currentUser.Name = user.Name;
            await _context.SaveChangesAsync();
            return currentUser;
        }
        
        private async Task<User> CreateUser(string slackUserId)
        {
            var user = new User
            {
                Karma = new Karma
                {
                    HighestKarma = 0,
                    HighestKarmaDate = DateTime.Now,
                    KarmaCount = 0,
                    LowestKarma = 0,
                    LowestKarmaDate = DateTime.Now
                },
                SlackUserId = slackUserId
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
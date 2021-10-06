using System;

namespace KarmaBot.Models
{
    public class Karma
    {
        public long KarmaId { get; set; }
        public long KarmaCount { get; set; }
        public long HighestKarma { get; set; }
        public DateTime? HighestKarmaDate { get; set; }
        public long LowestKarma { get; set; }
        public DateTime? LowestKarmaDate { get; set; }
        public long PositiveKarmaGiven { get; set; }
        public long NegativeKarmaGiven { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
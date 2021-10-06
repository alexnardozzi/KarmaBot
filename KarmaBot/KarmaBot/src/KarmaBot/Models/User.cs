namespace KarmaBot.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string SlackUserId { get; set; }
        public virtual Karma Karma { get; set; }
    }
}
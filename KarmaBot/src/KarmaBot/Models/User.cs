namespace KarmaBot.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string SlackUserId { get; set; }
        public string Name { get; set; }
        public bool IsNonUser { get; set; }
        public virtual Karma Karma { get; set; }
    }
}
namespace PaidGame.Server.Models
{
    public class Referral
    {
        public string Nickname { get; set; }

        public int MoneyAvailable { get; set; }

        public Referral(string nickname, int moneyAvailable = 0)
        {
            Nickname = nickname;
            MoneyAvailable = moneyAvailable;
        }
    }
}

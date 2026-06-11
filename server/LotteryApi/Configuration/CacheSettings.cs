namespace LotteryApi.Configuration
{
    public class CacheSettings
    {
        public int SlidingExpirationMinutes { get; set; } = 5;
        public string Prefix { get; set; } = "Lottery:";
    }
}

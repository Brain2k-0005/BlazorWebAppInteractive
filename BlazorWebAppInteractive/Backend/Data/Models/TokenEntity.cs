namespace BlazorWebAppInteractive.Backend.Data.Models
{
    public class TokenEntity
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
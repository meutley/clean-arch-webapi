namespace SourceName.Application.Common.Configuration
{
    public class AuthenticationConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int TokenLifetimeInSeconds { get; set; }
        public string TokenSecret { get; set; }
    }
}
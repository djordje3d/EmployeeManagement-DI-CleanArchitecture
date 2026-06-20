using Newtonsoft.Json;

namespace Zaposleni_Clean_MVC_API.Data
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}

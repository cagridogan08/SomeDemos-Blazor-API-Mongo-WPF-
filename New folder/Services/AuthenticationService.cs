using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WpfAppWithRedisCache.Services
{
    public interface IAuthenticationService
    {
        Task<string?> GetAuthTokenAsync(UserLoginDto userLoginDto);
        Task<string?> GetValidTokenAsync(UserLoginDto user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private string? _cachedToken;
        private DateTime _tokenExpiration;
        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetAuthTokenAsync(UserLoginDto userLoginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", userLoginDto);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseContent);
            var userToken = responseObject["value"]?["userToken"]?.ToString();
            return userToken;
        }
        public async Task<string?> GetValidTokenAsync(UserLoginDto user)
        {
            if (string.IsNullOrEmpty(_cachedToken) || DateTime.UtcNow > _tokenExpiration)
            {
                var tokenInfo = await GetAuthTokenAsync(user);
                _cachedToken = tokenInfo;
                _tokenExpiration = DateTimeOffset.Now.AddHours(12.0).Date;
            }
            return _cachedToken;
        }


    }

}

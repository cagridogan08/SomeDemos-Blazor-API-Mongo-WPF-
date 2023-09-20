using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using WpfAppWithRedisCache.Services;

namespace WpfAppWithRedisCache
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthTokenHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authenticationService.GetValidTokenAsync(UserLoginDto.GetDefaultUser());

            if (!string.IsNullOrEmpty(token) && request.Headers.Authorization is null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}

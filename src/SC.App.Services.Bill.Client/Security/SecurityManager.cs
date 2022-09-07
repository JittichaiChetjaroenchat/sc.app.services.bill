using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Security.Client;

namespace SC.App.Services.Bill.Client.Security
{
    public class SecurityManager : ISecurityManager
    {
        private readonly ISecurityClient _securityClient;

        public SecurityManager(
            ISecurityClient securityClient)
        {
            _securityClient = securityClient;
        }

        public async Task<ResponseOfValidateAccessTokenResponse> ValidateTokenAsync(HttpRequest request, string accessToken)
        {
            _securityClient.SetAuthorization(request.GetAuthorization());
            _securityClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new ValidateAccessToken
            {
                Access_token = accessToken
            };

            return await _securityClient.Tokens_ValidateAsync(payload);
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Security.Client;

namespace SC.App.Services.Bill.Client.Security
{
    public interface ISecurityManager
    {
        Task<ResponseOfValidateAccessTokenResponse> ValidateTokenAsync(HttpRequest request, string accessToken);
    }
}
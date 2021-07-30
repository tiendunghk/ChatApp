using System.Threading.Tasks;
using TeamApp.Application.DTOs.Account;
using TeamApp.Application.Wrappers;

namespace TeamApp.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<ApiResponse<TokenModel>> Refresh(TokenModel tokenModel);
        Task<ApiResponse<string>> IsLogin(string accessToken, string refreshToken);
    }
}

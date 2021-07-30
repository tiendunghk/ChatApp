using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeamApp.Application.DTOs.Account;
using TeamApp.Application.Exceptions;
using TeamApp.Application.Interfaces;
using TeamApp.Application.Wrappers;
using TeamApp.Domain.Settings;
using TeamApp.Infrastructure.Persistence.Entities;
using TeamApp.Infrastructure.Persistence.Helpers;
using Task = System.Threading.Tasks.Task;

namespace TeamApp.Infrastructure.Persistence.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly TeamAppContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<User> signInManager,
            TeamAppContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var check = false;
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                var newUser = new User
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.Email,
                    CreatedAt = DateTime.UtcNow,
                };

                var resultCreate = await _userManager.CreateAsync(newUser, "999999");
                if (resultCreate.Succeeded)
                {
                    check = true;
                    user = await _userManager.FindByEmailAsync(request.Email);
                }
                else
                {
                    throw new ApiException("Error");
                }
            }
            else
            {
                check = true;
            }


            if (check)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, "999999", false, lockoutOnFailure: false);


                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                AuthenticationResponse response = new AuthenticationResponse();
                response.Id = user.Id;
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.Email = user.Email;
                response.UserAvatar = string.IsNullOrEmpty(user.ImageUrl) ? $"https://ui-avatars.com/api/?name={user.FullName}" : user.ImageUrl;
                response.FullName = user.FullName;

                var refreshToken = GenerateRefreshToken();
                refreshToken.UserId = user.Id;
                response.RefreshToken = refreshToken.Token;

                await _dbContext.RefreshToken.AddAsync(refreshToken);
                await _dbContext.SaveChangesAsync();

                return new ApiResponse<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
            }

            throw new ApiException("Error");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
            };
        }

        public async Task<ApiResponse<TokenModel>> Refresh(TokenModel tokenModel)
        {
            var refreshToken = tokenModel.RefreshToken;
            var accesToken = tokenModel.AccessToken;
            var principal = GetPrincipalFromExpiredToken(accesToken);

            var userId = principal.Claims.ToList()[1].Value;
            var user = await _dbContext.User.FindAsync(userId);

            var tokenObj = await _dbContext.RefreshToken.Where(x => x.UserId == userId
            && x.Token == refreshToken && x.Expires > DateTime.UtcNow).FirstOrDefaultAsync();

            if (tokenObj == null)
                throw new ApiException("Refresh token not match for this user");

            var jwtSecurityToken = await GenerateJWToken(user);
            var refreshObj = GenerateRefreshToken();
            refreshObj.UserId = userId;

            await _dbContext.RefreshToken.AddAsync(refreshObj);
            await _dbContext.SaveChangesAsync();

            _dbContext.RefreshToken.Remove(tokenObj);
            await _dbContext.SaveChangesAsync();

            var outPut = new ApiResponse<TokenModel>
            {
                Succeeded = true,
                Data = new TokenModel
                {
                    RefreshToken = StringHelper.EncryptString(refreshObj.Token),
                    AccessToken = StringHelper.EncryptString(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)),
                }
            };

            return outPut;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }



        public async Task<ApiResponse<string>> IsLogin(string accessToken, string refreshToken)
        {
            var outPut = "Auth";

            //không có token trong cookie
            if (string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(refreshToken))
                outPut = "UnAuth";

            if (!string.IsNullOrEmpty(accessToken) && string.IsNullOrEmpty(refreshToken))
                outPut = "UnAuth";

            if (string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
                outPut = "UnAuth";
            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                var principal = GetPrincipalFromExpiredToken(StringHelper.DecryptString(accessToken)).Claims.ToList();
                var userId = principal[3].Value;
                var refreshDec = StringHelper.DecryptString(refreshToken);
                var refreshEntity = await _dbContext.RefreshToken.Where(x => x.UserId == userId && x.Token == refreshDec).FirstOrDefaultAsync();
                if (refreshEntity == null)
                    outPut = "UnAuth";
            }
            return new ApiResponse<string>
            {
                Succeeded = true,
                Data = outPut,
                Message = null,
            };
        }


    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.Account;
using TeamApp.Application.Exceptions;
using TeamApp.Application.Interfaces;
using TeamApp.Application.Wrappers;
using TeamApp.Domain.Settings;
using TeamApp.Infrastructure.Persistence.Helpers;

namespace TeamApp.WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<MyAppSettings> _config;
        public AccountController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IOptions<MyAppSettings> config)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(ApiResponse<AuthenticationResponse>), 200)]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var outPut = await _accountService.AuthenticateAsync(request);
            return Ok(outPut);
        }

        /// <summary>
        /// Refresh access token API
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<TokenModel>), 200)]
        public async Task<IActionResult> Refresh([FromBody]TokenModel tokenModel)
        {           
            var outPut = await _accountService.Refresh(tokenModel);
            return Ok(outPut);
        }

        /// <summary>
        /// Check login API
        /// </summary>
        /// <returns></returns>
        [HttpGet("is-login")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> IsLogin()
        {
            var token = HttpContext.Request.Cookies["access_token"];
            var refreshtoken = HttpContext.Request.Cookies["refresh_token"];
            var backup = HttpContext.Request.Cookies["backup"];
            if (string.IsNullOrEmpty(backup))
                throw new ApiException("Đã logout");

            return Ok(await _accountService.IsLogin(token, refreshtoken));
        }
    }
}
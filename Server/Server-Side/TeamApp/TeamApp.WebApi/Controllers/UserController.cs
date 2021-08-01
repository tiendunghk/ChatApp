using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.User;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Application.Wrappers;

namespace TeamApp.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("search")]
        [ProducesDefaultResponseType(typeof(ApiResponse<List<UserResponse>>))]
        public async Task<IActionResult> SearchUser([FromQuery] UserSearchModel searchModel)
        {
            var outPut = await _repo.SearchUser(searchModel.UserId, searchModel.Keyword);
            return Ok(new ApiResponse<List<UserResponse>>
            {
                Data = outPut,
                Succeeded = outPut != null
            });
        }
    }
}

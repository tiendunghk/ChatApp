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


 

        /// <summary>
        /// Search all users in all group user joined API
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet("search-user")]
        [ProducesDefaultResponseType(typeof(ApiResponse<List<UserResponse>>))]
        public async Task<IActionResult> SearchUser([FromQuery] UserSearchModel searchModel)
        {
            var outPut = await _repo.SearchUser(searchModel.UserId, searchModel.Keyword, searchModel.IsEmail);
            return Ok(new ApiResponse<List<UserResponse>>
            {
                Data = outPut,
                Succeeded = outPut != null
            });
        }

        /// <summary>
        /// Search users to add to exists chat API
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet("search-user-add-chat")]
        [ProducesDefaultResponseType(typeof(ApiResponse<List<UserResponse>>))]
        public async Task<IActionResult> SearchUserExistsAddChat([FromQuery] UserExistsChatAddModel searchModel)
        {
            var outPut = await _repo.SearchUserAddToExistsChat(searchModel.UserId, searchModel.GroupChatId, searchModel.Keyword, searchModel.IsEmail);
            return Ok(new ApiResponse<List<UserResponse>>
            {
                Data = outPut,
                Succeeded = outPut != null
            });
        }
    }
}

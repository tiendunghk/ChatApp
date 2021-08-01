using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.GroupChat;
using TeamApp.Application.Exceptions;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Application.Wrappers;

namespace TeamApp.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/groupchat")]
    public class GroupChatController : ControllerBase
    {
        private readonly IGroupChatRepository _repo;
        public GroupChatController(IGroupChatRepository repo)
        {
            _repo = repo;
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var res = await _repo.GetAllByUserId(userId);

            var outPut = new ApiResponse<List<GroupChatResponse>>
            {
                Data = res,
                Succeeded = true,
            };

            return Ok(outPut);
        }

        /// <summary>
        /// Add group chat API
        /// </summary>
        /// <param name="grChatReq"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> AddGroupChat([FromBody] GroupChatRequest grChatReq)
        {
            var res = await _repo.AddGroupChat(grChatReq);

            var outPut = new ApiResponse<string>
            {
                Data = res == null ? null : res,
                Succeeded = res == null ? false : true,
                Message = res == null ? "Lỗi khi thêm" : null,
            };

            return Ok(outPut);
        }


        /// <summary>
        /// Check exists chat 1 vs 1 API
        /// </summary>
        /// <param name="chatExists"></param>
        /// <returns></returns>
        [HttpPost("check-double-exists")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> CheckDoubleExists([FromBody] CheckDoubleGroupChatExists chatExists)
        {
            var res = await _repo.CheckDoubleGroupChatExists(chatExists);

            var outPut = new ApiResponse<object>
            {
                Data = res,
                Succeeded = true,
            };

            return Ok(outPut);
        }
    }
}

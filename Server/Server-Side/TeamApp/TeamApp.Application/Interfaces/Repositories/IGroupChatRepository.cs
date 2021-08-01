using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.GroupChat;

namespace TeamApp.Application.Interfaces.Repositories
{
    public interface IGroupChatRepository
    {
        Task<List<GroupChatResponse>> GetAllByUserId(string userId);
        Task<GroupChatResponse> AddGroupChat(GroupChatRequest grChatReq);  
        Task<CheckResponse> CheckDoubleGroupChatExists(CheckDoubleGroupChatExists chatExists);

    }
}

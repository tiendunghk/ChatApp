using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.GroupChat;

namespace TeamApp.Application.Interfaces.Repositories
{
    public interface IGroupChatRepository
    {
        Task<CustomListGroupChatResponse> GetAllByUserId(GroupChatSearch search);
        Task<string> AddGroupChat(GroupChatRequest grChatReq);  
        Task<object> CheckDoubleGroupChatExists(CheckDoubleGroupChatExists chatExists);

    }
}

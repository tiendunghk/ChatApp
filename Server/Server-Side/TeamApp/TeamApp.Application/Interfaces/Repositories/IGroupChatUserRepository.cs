using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.GroupChatUser;

namespace TeamApp.Application.Interfaces.Repositories
{
    public interface IGroupChatUserRepository
    {
        Task<string> AddGroupChatUser(GroupChatUserRequest grChatUserReq);
    }
}

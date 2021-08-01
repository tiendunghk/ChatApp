using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.GroupChatUser;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Infrastructure.Persistence.Entities;
using TeamApp.Application.Utils;

namespace TeamApp.Infrastructure.Persistence.Repositories
{
    public class GroupChatUserRepository : IGroupChatUserRepository
    {
        private readonly TeamAppContext _dbContext;

        public GroupChatUserRepository(TeamAppContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> AddGroupChatUser(GroupChatUserRequest grChatUserReq)
        {
            var entity = new GroupChatUser
            {
                GroupChatUserId = Guid.NewGuid().ToString(),
                GroupChatUserUserId = grChatUserReq.GroupChatUserUserId,
                GroupChatUserGroupChatId = grChatUserReq.GroupChatUserGroupChatId,
            };

            await _dbContext.GroupChatUser.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.GroupChatUserId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Infrastructure.Persistence.Entities;
using Task = System.Threading.Tasks.Task;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamApp.Application.DTOs.GroupChat;
using TeamApp.Application.Utils;
using TeamApp.Application.DTOs.User;
using System.Data;
using TeamApp.Infrastructure.Persistence.Helpers;
using Microsoft.AspNetCore.SignalR;
using TeamApp.Infrastructure.Persistence.Hubs.Chat;
using System.Collections.ObjectModel;

namespace TeamApp.Infrastructure.Persistence.Repositories
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly TeamAppContext _dbContext;
        private readonly IHubContext<HubChatClient, IHubChatClient> _chatHub;
        private readonly IGroupChatUserRepository _groupChatUserRepository;
        public GroupChatRepository(TeamAppContext dbContext, IHubContext<HubChatClient, IHubChatClient> chatHub, IGroupChatUserRepository groupChatUserRepository)
        {
            _dbContext = dbContext;
            _chatHub = chatHub;
            _groupChatUserRepository = groupChatUserRepository;
        }
        public async Task<GroupChatResponse> AddGroupChat(GroupChatRequest grChatReq)
        {
            var check = await CheckDoubleGroupChatExists(new Application.DTOs.GroupChat.CheckDoubleGroupChatExists
            {
                UserOneId = grChatReq.UserOneId,
                UserTwoId = grChatReq.UserTwoId,
            });

            var userTwo = await _dbContext.User.FindAsync(grChatReq.UserTwoId);

            if (check.Exists)
            {
                var groupChat = await _dbContext.GroupChat.FindAsync(check.GroupId);
                return new GroupChatResponse
                {
                    GroupChatId = groupChat.GroupChatId,
                    GroupChatName = userTwo.FullName,
                    GroupChatUpdatedAt = DateTime.UtcNow,
                    GroupAvatar = string.IsNullOrEmpty(userTwo.ImageUrl) ? $"https://ui-avatars.com/api/?name={userTwo.FullName}" : userTwo.ImageUrl,
                };
            }

            else
            {
                var entity = new GroupChat
                {
                    GroupChatId = Guid.NewGuid().ToString(),
                    GroupChatCreatedAt = DateTime.UtcNow,
                };

                await _dbContext.GroupChat.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                await _groupChatUserRepository.AddGroupChatUser(new Application.DTOs.GroupChatUser.GroupChatUserRequest
                {
                    GroupChatUserUserId = grChatReq.UserOneId,
                    GroupChatUserGroupChatId = entity.GroupChatId,
                });

                await _groupChatUserRepository.AddGroupChatUser(new Application.DTOs.GroupChatUser.GroupChatUserRequest
                {
                    GroupChatUserUserId = grChatReq.UserTwoId,
                    GroupChatUserGroupChatId = entity.GroupChatId,
                });

                await PushNoti(entity.GroupChatId, grChatReq);

                return new GroupChatResponse
                {
                    GroupChatId = entity.GroupChatId,
                    GroupChatName = userTwo.FullName,
                    GroupChatUpdatedAt = DateTime.UtcNow,
                    GroupAvatar = string.IsNullOrEmpty(userTwo.ImageUrl) ? $"https://ui-avatars.com/api/?name={userTwo.FullName}" : userTwo.ImageUrl,
                };
            }
        }

        async Task PushNoti(string groupId, GroupChatRequest groupChatRequest)
        {
            var groupChat = await _dbContext.GroupChat.FindAsync(groupId);
            var userOne = await _dbContext.User.FindAsync(groupChatRequest.UserOneId);
            var userTwo = await _dbContext.User.FindAsync(groupChatRequest.UserTwoId);

            var payloadUser1 = new GroupChatResponse
            {
                GroupChatId = groupId,
                GroupChatName = userTwo.FullName,
                GroupChatUpdatedAt = groupChat.GroupChatCreatedAt.FormatTime(),
                GroupAvatar = string.IsNullOrEmpty(userTwo.ImageUrl) ? $"https://ui-avatars.com/api/?name={userTwo.FullName}" : userTwo.ImageUrl,
            };

            var payloadUser2 = new GroupChatResponse
            {
                GroupChatId = groupId,
                GroupChatName = userOne.FullName,
                GroupChatUpdatedAt = groupChat.GroupChatCreatedAt.FormatTime(),
                GroupAvatar = string.IsNullOrEmpty(userOne.ImageUrl) ? $"https://ui-avatars.com/api/?name={userOne.FullName}" : userOne.ImageUrl,
            };

            var userOneConnections = await (from u in _dbContext.UserConnection.AsNoTracking()
                                            where u.UserId == groupChatRequest.UserOneId
                                            select u.ConnectionId).ToListAsync();

            var userTwoConnections = await (from u in _dbContext.UserConnection.AsNoTracking()
                                            where u.UserId == groupChatRequest.UserTwoId
                                            select u.ConnectionId).ToListAsync();

            await _chatHub.Clients.Clients(userOneConnections).NewGroupChat(payloadUser1);
            await _chatHub.Clients.Clients(userTwoConnections).NewGroupChat(payloadUser2);
        }

        public async Task<CheckResponse> CheckDoubleGroupChatExists(CheckDoubleGroupChatExists chatExists)
        {
            var query = "SELECT g.group_chat_id " +
                        "FROM group_chat g " +
                        "JOIN group_chat_user gu ON (gu.group_chat_user_group_chat_id = g.group_chat_id) " +
                        "GROUP BY g.group_chat_id " +
                        $"HAVING (SUM(gu.group_chat_user_user_id IN ('{chatExists.UserOneId}','{chatExists.UserTwoId}')) = 2) " +
                        $"AND (SUM(gu.group_chat_user_user_id NOT IN ('{chatExists.UserOneId}','{chatExists.UserTwoId}')) = 0) ";
            var results = await RawQuery.RawSqlQuery(_dbContext, query
                , x => (string)x[0]);


            if (results.Count() == 1)
            {
                return new CheckResponse
                {
                    Exists = true,
                    GroupId = results[0],
                };
            }

            return new CheckResponse
            {
                Exists = false,
                GroupId = null,
            };
        }

        public async Task<List<GroupChatResponse>> GetAllByUserId(string userId)
        {
            var response = new List<GroupChatResponse>();

            var outPut = await (from gc in _dbContext.GroupChat.AsNoTracking()
                                join grc in _dbContext.GroupChatUser.AsNoTracking() on gc.GroupChatId equals grc.GroupChatUserGroupChatId
                                where grc.GroupChatUserUserId == userId
                                orderby gc.GroupChatCreatedAt descending
                                select new { gc, grc }).ToListAsync();


            foreach (var gr in outPut)
            {
                //get lastest message
                var lastest = await (from m in _dbContext.Message.AsNoTracking()
                                     where m.MessageGroupChatId == gr.gc.GroupChatId
                                     orderby m.MessageCreatedAt descending
                                     select m).FirstOrDefaultAsync();

                response.Add(new GroupChatResponse
                {
                    GroupChatId = gr.gc.GroupChatId,
                    GroupChatName = gr.gc.GroupChatName,
                    GroupChatUpdatedAt = lastest == null ? gr.gc.GroupChatCreatedAt.FormatTime() : lastest.MessageCreatedAt.FormatTime(),
                    LastestMes = lastest == null ? null : lastest.MessageContent,
                });
            }

            response = response.OrderByDescending(x => x.GroupChatUpdatedAt).ToList();

            foreach (var res in response)
            {
                var user = await (from grc in _dbContext.GroupChatUser.AsNoTracking()
                                  join u in _dbContext.User.AsNoTracking() on grc.GroupChatUserUserId equals u.Id
                                  where grc.GroupChatUserUserId != userId && grc.GroupChatUserGroupChatId == res.GroupChatId
                                  select new { u.FullName, u.ImageUrl }).FirstOrDefaultAsync();

                res.GroupChatName = user.FullName;
                res.GroupAvatar = string.IsNullOrEmpty(user.ImageUrl) ? $"https://ui-avatars.com/api/?name={user.FullName}" : user.ImageUrl;
            }

            return response;
        }
    }
}

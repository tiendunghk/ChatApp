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
        public GroupChatRepository(TeamAppContext dbContext, IHubContext<HubChatClient, IHubChatClient> chatHub)
        {
            _dbContext = dbContext;
            _chatHub = chatHub;
        }
        public async Task<string> AddGroupChat(GroupChatRequest grChatReq)
        {
            var entity = new GroupChat
            {
                GroupChatId = string.IsNullOrEmpty(grChatReq.GroupChatId) ? Guid.NewGuid().ToString() : grChatReq.GroupChatId,
                GroupChatName = grChatReq.GroupChatName,
                GroupChatCreatedAt = DateTime.UtcNow,
            };

            await _dbContext.GroupChat.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.GroupChatId;
        }
        

        public async Task<object> CheckDoubleGroupChatExists(CheckDoubleGroupChatExists chatExists)
        {
            /*var lists = new List<string> { chatExists.UserOneId, chatExists.UserTwoId };
            var find = await (from gc in _dbContext.GroupChat.AsNoTracking()
                              join gru in _dbContext.GroupChatUser.AsNoTracking() on gc.GroupChatId equals gru.GroupChatUserGroupChatId
                              where gc.GroupChatType == "double" && lists.Contains(gru.GroupChatUserGroupChatId)
                              select gc).ToListAsync();*/

            var query = "SELECT g.group_chat_id " +
                        "FROM group_chat g " +
                        "JOIN group_chat_user gu ON (gu.group_chat_user_group_chat_id = g.group_chat_id) " +
                        "where g.group_chat_type='double' " +
                        "GROUP BY g.group_chat_id " +
                        $"HAVING (SUM(gu.group_chat_user_user_id IN ('{chatExists.UserOneId}','{chatExists.UserTwoId}')) = 2) " +
                        $"AND (SUM(gu.group_chat_user_user_id NOT IN ('{chatExists.UserOneId}','{chatExists.UserTwoId}')) = 0) ";
            var results = await RawQuery.RawSqlQuery(_dbContext, query
                , x => (string)x[0]);


            var user2 = await _dbContext.User.FindAsync(chatExists.UserTwoId);
            var user2Res = new UserResponse
            {
                UserId = user2.Id,
                UserFullname = user2.FullName,
                UserImageUrl = user2.ImageUrl,
            };
            if (results.Count() == 1)
            {
                return new
                {
                    Exists = true,
                    GroupChatId = results[0],
                    UserTwo = user2Res,
                };
            }

            return new
            {
                Exists = false,
                GroupChatId = string.Empty,
                UserTwo = user2Res,
            };
        }

        public async Task<CustomListGroupChatResponse> GetAllByUserId(GroupChatSearch search)
        {
            var responseCustom = new CustomListGroupChatResponse();

            var query = from gc in _dbContext.GroupChat.AsNoTracking()
                        join grc in _dbContext.GroupChatUser.AsNoTracking() on gc.GroupChatId equals grc.GroupChatUserGroupChatId
                        select new { gc, grc };

            var outputQuery = query.Where(x => x.grc.GroupChatUserUserId == search.UserId).OrderByDescending(x => x.gc.GroupChatCreatedAt);

            var outPut = await outputQuery.ToListAsync();

            var lists = new List<GroupChatResponse>();
            foreach (var gr in outPut)
            {
                //get lastest message
                var lastest = await (from m in _dbContext.Message.AsNoTracking()
                                     where m.MessageGroupChatId == gr.gc.GroupChatId
                                     orderby m.MessageCreatedAt descending
                                     select m).FirstOrDefaultAsync();

                lists.Add(new GroupChatResponse
                {
                    GroupChatId = gr.gc.GroupChatId,
                    GroupChatName = gr.gc.GroupChatName,
                    GroupChatUpdatedAt = lastest == null ? gr.gc.GroupChatCreatedAt.FormatTime() : lastest.MessageCreatedAt.FormatTime(),
                    GroupAvatar = gr.gc.GroupChatImageUrl,
                    LastestMes = lastest == null ? null : lastest.MessageContent,
                });
            }

            lists = lists.OrderByDescending(x => x.GroupChatUpdatedAt).ToList();

            foreach (var l in lists)
            {
                    var user = await (from grc in _dbContext.GroupChatUser.AsNoTracking()
                                      join u in _dbContext.User.AsNoTracking() on grc.GroupChatUserUserId equals u.Id
                                      where grc.GroupChatUserUserId != search.UserId && grc.GroupChatUserGroupChatId == l.GroupChatId
                                      select new { u.FullName, u.ImageUrl }).FirstOrDefaultAsync();

                    l.GroupChatName = !string.IsNullOrEmpty(l.GroupChatName) ? l.GroupChatName : user.FullName;
                    l.GroupAvatar = string.IsNullOrEmpty(l.GroupAvatar) ? (string.IsNullOrEmpty(user.ImageUrl) ? $"https://ui-avatars.com/api/?name={user.FullName}" : user.ImageUrl) : l.GroupAvatar;             
            }

            if (search.IsSearch)
            {
                if (!string.IsNullOrEmpty(search.KeyWord))
                {
                    var xoadau = search.KeyWord.UnsignUnicode();
                    lists = lists.Where(x => x.GroupChatName.UnsignUnicode().Contains(xoadau)).Select(x => x).ToList();
                }
            }

            responseCustom.GroupChats = lists;

            if (lists.Where(x => x.GroupChatId == search.CurrentGroup).Count() > 0)
                responseCustom.CurrentGroup = search.CurrentGroup;
            else
            {
                if (lists.Count > 0)
                    responseCustom.CurrentGroup = lists[0].GroupChatId;
            }

            return responseCustom;
        }
    }
}

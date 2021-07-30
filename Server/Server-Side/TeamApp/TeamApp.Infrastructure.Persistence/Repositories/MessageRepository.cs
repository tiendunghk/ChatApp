﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamApp.Application.Filters;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Application.Wrappers;
using TeamApp.Infrastructure.Persistence.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamApp.Application.DTOs.Message;
using TeamApp.Application.Utils;
using System.Web;

namespace TeamApp.Infrastructure.Persistence.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly TeamAppContext _dbContext;

        public MessageRepository(TeamAppContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> AddMessage(MessageRequest msgReq)
        {
            var entity = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageUserId = msgReq.MessageUserId,
                MessageGroupChatId = msgReq.MessageGroupChatId,
                MessageContent = msgReq.MessageContent,
                MessageCreatedAt = DateTime.UtcNow,
            };

            await _dbContext.Message.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.MessageId;
        }

        public async Task<PagedResponse<MessageResponse>> GetPaging(MessageRequestParameter parameter)
        {
            var group = await _dbContext.GroupChat.FindAsync(parameter.GroupId);
            if (group == null) return null;

            var gru = await (from gruser in _dbContext.GroupChatUser.AsNoTracking()
                             where gruser.GroupChatUserGroupChatId == parameter.GroupId &&
                             gruser.GroupChatUserUserId == parameter.UserId
                             select gruser).FirstOrDefaultAsync();

            if (gru == null)
                throw new KeyNotFoundException("Not found groupchat");

            var query = from m in _dbContext.Message
                        join u in _dbContext.User on m.MessageUserId equals u.Id
                        where m.MessageGroupChatId == parameter.GroupId
                        select new { u, m };

            var outPut = query.OrderByDescending(x => x.m.MessageCreatedAt).Skip(parameter.SkipItems)
                .Take(parameter.PageSize);

            var items = await outPut.Select(x => new MessageResponse
            {
                MessageId = x.m.MessageId,
                MessageUserId = x.m.MessageUserId,
                MessageGroupChatId = x.m.MessageGroupChatId,
                MessageContent = x.m.MessageContent,
                MessageCreatedAt = x.m.MessageCreatedAt.FormatTime(),
                MessengerUserAvatar = string.IsNullOrEmpty(x.u.ImageUrl) ? $"https://ui-avatars.com/api/?name={x.u.FullName}" : x.u.ImageUrl,
                MessengerUserName = x.u.FullName,
            }).ToListAsync();

            items.Reverse();

            return new PagedResponse<MessageResponse>(items, parameter.PageSize, items.Count);
        }
    }
}

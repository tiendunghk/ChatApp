using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamApp.Application.DTOs.User;
using TeamApp.Application.Interfaces.Repositories;
using TeamApp.Infrastructure.Persistence.Entities;
using TeamApp.Application.Utils;
using Microsoft.AspNetCore.SignalR;


namespace TeamApp.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TeamAppContext _dbContext;

        public UserRepository(TeamAppContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<UserResponse>> SearchUser(string userId, string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
                return new List<UserResponse>();

            keyWord = keyWord.UnsignUnicode();

            var users = await (from u in _dbContext.User.AsNoTracking()
                               where u.Id != userId
                               select u).ToListAsync();
            users = users.Where(u => u.FullName.UnsignUnicode().Contains(keyWord)).ToList();

            return users.Select(x => new UserResponse
            {
                UserId = x.Id,
                UserFullname = x.FullName,
                UserImageUrl = string.IsNullOrEmpty(x.ImageUrl) ? $"https://ui-avatars.com/api/?name={x.FullName}" : x.ImageUrl,
            }).ToList();
        }
    }
}

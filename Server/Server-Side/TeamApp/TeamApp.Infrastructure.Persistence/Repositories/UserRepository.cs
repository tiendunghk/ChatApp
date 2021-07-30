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
               

        public async Task<List<UserResponse>> SearchUser(string userId, string keyWord, bool email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(keyWord);
                if (addr.Address == keyWord)
                    email = true;
                else
                    email = false;
            }
            catch
            {
                email = false;
            }
            var query = "";
            if (!email)
            {
                query = "SELECT * FROM user " +
                $"where user.user_id <> '{userId}' and user.user_fullname like '%{keyWord}%'";

                var newQuery = "select distinct u.user_id, u.user_fullname, u.user_image_url " +
                              "from user u " +
                              "join " +
                              "(select distinct p.participation_user_id " +
                              "from participation p " +
                              "join " +
                              "(select t.team_id, p.participation_user_id " +
                              "from team t join participation p on t.team_id = p.participation_team_id " +
                              $"where p.participation_user_id = '{userId}' and p.participation_is_deleted = 0) teamIDs " +
                              "on p.participation_team_id = teamIDs.team_id) userIDs " +
                              "on u.user_id = userIDs.participation_user_id " +
                              $"where u.user_fullname like '%{keyWord}%'";

                query = newQuery;
            }
            else
            {
                query = "SELECT * FROM user " +
                $"where user.user_id <> '{userId}' and user.user_email = '{keyWord}'";

                var newQuery = "select distinct u.user_id, u.user_fullname, u.user_image_url " +
                              "from user u " +
                              "join " +
                              "(select distinct p.participation_user_id " +
                              "from participation p " +
                              "join " +
                              "(select t.team_id, p.participation_user_id " +
                              "from team t join participation p on t.team_id = p.participation_team_id " +
                              $"where p.participation_user_id = '{userId}' and p.participation_is_deleted = 0) teamIDs " +
                              "on p.participation_team_id = teamIDs.team_id) userIDs " +
                              "on u.user_id = userIDs.participation_user_id " +
                              $"where u.user_email = '{keyWord}'";

                query = newQuery;

            }

            var listUsers = await Helpers.RawQuery.RawSqlQuery(_dbContext, query, (x) => new User
            {
                Id = (string)x[0],
                FullName = (string)x[1],
                ImageUrl = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
            });
            //Console.WriteLine(query);

            //var outPut = await _dbContext.User.FromSqlRaw(query).ToListAsync();

            return listUsers.Select(x => new UserResponse
            {
                UserId = x.Id,
                UserFullname = x.FullName,
                UserImageUrl = string.IsNullOrEmpty(x.ImageUrl) ? $"https://ui-avatars.com/api/?name={x.FullName}" : x.ImageUrl,
            }).ToList();
        }

        public async Task<List<UserResponse>> SearchUserAddToExistsChat(string userId, string grChatId, string keyWord, bool isEmail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(keyWord);
                if (addr.Address == keyWord)
                    isEmail = true;
                else
                    isEmail = false;
            }
            catch
            {
                isEmail = false;
            }

            var query = "";
            if (!isEmail)
            {
                query = "SELECT * FROM user " +
                $"where user.user_id <> '{userId}' and user.user_fullname like '%{keyWord}%'";

                var newQuery = "select distinct u.user_id, u.user_fullname, u.user_image_url " +
                              "from user u " +
                              "join " +
                              "(select distinct p.participation_user_id " +
                              "from participation p " +
                              "join " +
                              "(select t.team_id, p.participation_user_id " +
                              "from team t join participation p on t.team_id = p.participation_team_id " +
                              $"where p.participation_user_id = '{userId}' and p.participation_is_deleted = 0) teamIDs " +
                              "on p.participation_team_id = teamIDs.team_id) userIDs " +
                              "on u.user_id = userIDs.participation_user_id " +
                              $"where u.user_fullname like '%{keyWord}%' and " +
                              $" u.user_id not in " +
                               "(select grc.group_chat_user_user_id " +
                                "from group_chat_user grc " +
                                $"where grc.group_chat_user_group_chat_id = '{grChatId}' and grc.group_chat_user_is_deleted = 0)";

                query = newQuery;
            }
            else
            {
                query = "SELECT * FROM user " +
                $"where user.user_id <> '{userId}' and user.user_email = '{keyWord}'";

                var newQuery = "select distinct u.user_id, u.user_fullname, u.user_image_url " +
                              "from user u " +
                              "join " +
                              "(select distinct p.participation_user_id " +
                              "from participation p " +
                              "join " +
                              "(select t.team_id, p.participation_user_id " +
                              "from team t join participation p on t.team_id = p.participation_team_id " +
                              $"where p.participation_user_id = '{userId}' and p.participation_is_deleted = 0) teamIDs " +
                              "on p.participation_team_id = teamIDs.team_id) userIDs " +
                              "on u.user_id = userIDs.participation_user_id " +
                              $"where u.user_email = '{keyWord}' and " +
                              $" u.user_id not in " +
                               "(select grc.group_chat_user_user_id " +
                                "from group_chat_user grc " +
                                $"where grc.group_chat_user_group_chat_id = '{grChatId}' and grc.group_chat_user_is_deleted = 0)";

                query = newQuery;

            }

            var listUsers = await Helpers.RawQuery.RawSqlQuery(_dbContext, query, (x) => new User
            {
                Id = (string)x[0],
                FullName = (string)x[1],
                ImageUrl = (x[2] == DBNull.Value) ? string.Empty : (string)x[2],
            });
            //Console.WriteLine(query);

            //var outPut = await _dbContext.User.FromSqlRaw(query).ToListAsync();

            return listUsers.Select(x => new UserResponse
            {
                UserId = x.Id,
                UserFullname = x.FullName,
                UserImageUrl = string.IsNullOrEmpty(x.ImageUrl) ? $"https://ui-avatars.com/api/?name={x.FullName}" : x.ImageUrl,
            }).ToList();
        }        
    }
}

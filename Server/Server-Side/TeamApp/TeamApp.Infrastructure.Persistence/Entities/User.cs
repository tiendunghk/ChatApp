using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TeamApp.Application.DTOs.Account;

namespace TeamApp.Infrastructure.Persistence.Entities
{
    public partial class User : IdentityUser
    {
        public User()
        {           
            Message = new HashSet<Message>();
            RefreshTokens = new List<RefreshToken>();
        }

        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }  

        public virtual ICollection<GroupChatUser> GroupChatUser { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<UserConnection> UserConnections { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamApp.Infrastructure.Persistence.Hubs.Chat
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Message { get; set; }
        public DateTime TimeSend { get; set; }
    }
}

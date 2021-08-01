using System;
using System.Collections.Generic;
using System.Text;

namespace TeamApp.Application.DTOs.Message
{
    public class MessageRequest
    {
        public string MessengerId { get; set; }
        public string MessageUserId { get; set; }
        public string MessageGroupChatId { get; set; }
        public string MessageContent { get; set; }
        public DateTime? MessageCreatedAt { get; set; }
    }
}

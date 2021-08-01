﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeamApp.Application.DTOs.Message
{
    public class MessageResponse
    {
        public string MessageId { get; set; }
        public string MessageUserId { get; set; }
        public string MessageGroupChatId { get; set; }
        public string MessageContent { get; set; }
        public DateTime? MessageCreatedAt { get; set; }
        public string MessengerUserAvatar { get; set; }
        public string MessengerUserName { get; set; }
        public bool? IsSender { get; set; }
    }
}

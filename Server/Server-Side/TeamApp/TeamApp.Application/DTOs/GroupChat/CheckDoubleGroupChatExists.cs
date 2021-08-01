using System;
using System.Collections.Generic;
using System.Text;

namespace TeamApp.Application.DTOs.GroupChat
{
    public class CheckResponse
    {
        public bool Exists { get; set; }
        public string GroupId { get; set; }
    }
    public class CheckDoubleGroupChatExists
    {
        public string UserOneId { get; set; }
        public string  UserTwoId { get; set; }
    }
}

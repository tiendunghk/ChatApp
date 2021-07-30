﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TeamApp.Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }        
        public string JWToken { get; set; }
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
        public string UserAvatar { get; set; }
    }
}

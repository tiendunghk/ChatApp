﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeamApp.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}

﻿using PartyMaker.Common.Request;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppRestorePasswordStoryContext : IRequest
    {
        public string Email { get; set; }
    }
}
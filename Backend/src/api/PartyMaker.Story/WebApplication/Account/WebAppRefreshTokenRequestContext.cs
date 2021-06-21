using PartyMaker.Common.Request;
using System;

namespace PartyMaker.Story.WebApplication.Account
{
    public class WebAppRefreshTokenRequestContext : IRequest
    {
        public Guid Token { get; set; }
    }
}

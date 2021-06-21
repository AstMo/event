using PartyMaker.Domain.Entities;
using System;
using System.Security.Claims;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppAuthenticateResponceDto : WebAppEntityDto
    {
        public bool WrongCredentials { get; set; }

        public string Username { get; set; }

        public EUserRole UserRole { get; set; }

        public string Token { get; set; }

        public long ExpiresTime { get; set; }

        public string RefreshToken { get; set; }
    }
}

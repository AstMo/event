using PartyMaker.Common.Account;
using PartyMaker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PartyMaker.Common.Impl.Admin
{
    public class IdentityService : IIdentityService
    {
        public ClaimsIdentity GetClaimsIdentity(Guid userId, EUserRole eUserRole, string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Role, eUserRole.ToString()),
                new Claim(ClaimTypes.UserData, username),
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "login", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}

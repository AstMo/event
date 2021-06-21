using PartyMaker.Domain.Entities;
using System;
using System.Security.Claims;

namespace PartyMaker.Common.Account
{
    public interface IIdentityService
    {
        ClaimsIdentity GetClaimsIdentity(Guid userId, EUserRole eUserRole, string username);
    }
}

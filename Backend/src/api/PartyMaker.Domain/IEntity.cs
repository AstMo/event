using System;

namespace PartyMaker.Domain
{
    interface IEntity
    {
        Guid? Id { get; set; }
    }
}

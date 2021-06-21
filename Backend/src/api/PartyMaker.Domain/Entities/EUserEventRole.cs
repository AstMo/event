using System;

namespace PartyMaker.Domain.Entities
{
    [Flags]
    public enum EUserEventRole
    {
        Admin = 0x01,
        CommandEditor = 0x02,
        BudgetEditor = 0x04,
        Partisipant = 0x08
    }
}

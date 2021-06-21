using bgTeam.DataAccess;

namespace PartyMaker.Configuration.Interfaces
{
    public interface IDatabaseSettings : IConnectionSetting
    {
        bool EnableLog { get; }
    }
}

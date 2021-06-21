namespace PartyMaker.Configuration.Interfaces
{
    public interface IImageStoreSetting
    {
        string Url { get; }

        string Username { get; }

        string Password { get; }

        string Path { get; }
    }
}

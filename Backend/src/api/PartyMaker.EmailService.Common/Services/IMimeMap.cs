namespace PartyMaker.EmailService.Common.Services
{
    public interface IMimeMap
    {
        void Initialize();
        string GetMimeType(string extension);
        string GetExtension(string mimeType);
    }
}

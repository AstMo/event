namespace PartyMaker.Dto.Queue
{
    public enum EEmailRequestType
    {
        Registration,
        Invite,
        RestorePassword
    }

    public class EmailRequestQueueDto : RequestQueueDto
    {
        public EEmailRequestType EmailRequestType { get; set; }

        public string Params { get; set; }
    }
}

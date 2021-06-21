namespace PartyMaker.Dto.WebApp
{
    public class WebAppResponseWithEntityDto<TEntityDto> : WebAppResponseDto
        where TEntityDto : WebAppEntityDto
    {
        public WebAppResponseWithEntityDto() : base()
        {

        }

        public TEntityDto Result { get; set; }
    }
}

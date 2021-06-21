namespace PartyMaker.Dto.WebApp
{
    public class WebAppResponseWithTableDto<TTableEntityDto, TEntityDto> : WebAppResponseDto
        where TTableEntityDto : WebAppTableDto<TEntityDto>
        where TEntityDto : WebAppEntityDto
    {
        public WebAppResponseWithTableDto() : base()
        {

        }

        public TTableEntityDto Result { get; set; }
    }
}

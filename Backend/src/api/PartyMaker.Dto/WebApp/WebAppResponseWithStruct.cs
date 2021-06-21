namespace PartyMaker.Dto.WebApp
{
    public class WebAppResponseWithStructDto<T> : WebAppResponseDto
        where T :  class
    {
        public WebAppResponseWithStructDto() : base()
        {

        }

        public T Result { get; set; }
    }
}

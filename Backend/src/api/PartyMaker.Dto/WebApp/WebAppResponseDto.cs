using System.Collections.Generic;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppResponseDto
    {
        public WebAppResponseDto()
        {
            IsSuccess = true;
            IsTimeout = false;
            IsInvalid = false;
            IsNotFound = false;
            Message = string.Empty;
        }

        public bool IsSuccess { get; set; }

        public bool IsTimeout { get; set; }

        public bool IsNotFound { get; set; }

        public string Message { get; set; }

        public IEnumerable<WebAppErrorDto> Errors { get; set; }

        public bool IsInvalid { get; set; }
    }
}

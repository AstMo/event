using Microsoft.AspNetCore.Http;
using PartyMaker.Common.Request;

namespace PartyMaker.Story.Image
{
    public class WebAppSaveImageStoryContext : IRequest
    {
        public IFormFile ImageFile { get; set; }
    }
}

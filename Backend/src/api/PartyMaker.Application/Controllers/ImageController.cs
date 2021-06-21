using bgTeam;
using PartyMaker.Dto.WebApp;
using PartyMaker.Story.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PartyMaker.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {
        private readonly IStoryBuilder _storyBuilder;
        private readonly IAppLogger _appLogger;

        public ImageController(IStoryBuilder storyBuilder,
            IAppLogger appLogger)
        {
            _storyBuilder = storyBuilder;
            _appLogger = appLogger;
        }

        [HttpPost]
        [Authorize]
        [Route("save")]
        [ProducesResponseType(typeof(WebAppImageResponseDto), 200)]
        [ProducesResponseType(typeof(WebAppImageResponseDto), 200)]
        public async Task<IActionResult> SaveImage([FromForm]IFormFile image)
        {
            _appLogger.Info("Get request to save  file");
            var result = await _storyBuilder.Build(new WebAppSaveImageStoryContext { ImageFile = image })
                .ReturnAsync<WebAppResponseWithEntityDto<WebAppImageResponseDto>>();

            return GetActionResult(result);
        }

        [HttpGet]
        [Authorize]
        [Route("load/{id}")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> GetFile(Guid id)
        {
            _appLogger.Info("Get request to get file");
            var result = await _storyBuilder.Build(new WebAppGetImageStoryContext { Id = id })
                 .ReturnAsync<(MemoryStream stream, string mime)>();
            result.stream.Seek(0, SeekOrigin.Begin);
            byte[] fileContent = new byte[result.stream.Length];
            await result.stream.ReadAsync(fileContent);
            return new FileContentResult(fileContent, result.mime);
        }
    }
}

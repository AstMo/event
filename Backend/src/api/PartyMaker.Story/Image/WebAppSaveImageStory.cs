using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using FluentFTP;
using System;
using System.Threading.Tasks;

namespace PartyMaker.Story.Image
{
    public class WebAppSaveImageStory : IStory<WebAppSaveImageStoryContext, WebAppResponseWithEntityDto<WebAppImageResponseDto>>
    {
        private readonly IAppLogger _appLogger;
        private readonly IImageStoreSetting _imageStoreSettings;
        private readonly ICrudService _crudService;
        private readonly FtpClient _ftpClient;

        public WebAppSaveImageStory(
            IImageStoreSetting imageStoreSettings,
            ICrudService crudService,
            IAppLogger appLogger)
        {
            _appLogger = appLogger;
            _imageStoreSettings = imageStoreSettings;
            _crudService = crudService;
            _ftpClient = new FtpClient(_imageStoreSettings.Url)
            {
                Credentials = new System.Net.NetworkCredential(_imageStoreSettings.Username, _imageStoreSettings.Password)
            };
        }

        public WebAppResponseWithEntityDto<WebAppImageResponseDto> Execute(WebAppSaveImageStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<WebAppResponseWithEntityDto<WebAppImageResponseDto>> ExecuteAsync(WebAppSaveImageStoryContext context)
        {
            _appLogger.Info($"Starting story for save image file {context.ImageFile.FileName}");
            try
        {       await _ftpClient.ConnectAsync();

                _appLogger.Info($"Starting write file for save image to file {_imageStoreSettings.Url} - {context.ImageFile.Length}");
                var filePath = $"{_imageStoreSettings.Path}/{Guid.NewGuid()}-{context.ImageFile.FileName}";
                var response = await _ftpClient.UploadAsync(context.ImageFile.OpenReadStream(), filePath);

                if (response.IsSuccess())
                {
                    _appLogger.Info("End writing to file");

                    var fileObject = new Domain.Entities.Image
                    {
                        Filename = context.ImageFile.FileName,
                        RealPath = filePath,
                    };
                    fileObject.MarkAsNew();
                    await _crudService.InsertAsync(fileObject);

                    _appLogger.Info("Success operation save file");
                    return new WebAppResponseWithEntityDto<WebAppImageResponseDto>
                    {
                        Result = new WebAppImageResponseDto
                        {
                            FileId = fileObject.Id
                        },
                        IsSuccess = true,
                        IsTimeout = false,
                        Message = string.Empty,
                    };
                }
                await _ftpClient.DisconnectAsync();

                return new WebAppResponseWithEntityDto<WebAppImageResponseDto>
                {
                    IsSuccess = false,
                    IsTimeout = false,
                    Message = response.ToString(),
                };
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                await _ftpClient.DisconnectAsync();
                return new WebAppResponseWithEntityDto<WebAppImageResponseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    IsTimeout = false,
                };
            }
        }
    }
}

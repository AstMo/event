using bgTeam;
using bgTeam.DataAccess;
using PartyMaker.Common.Impl.ErrorProvider;
using PartyMaker.Configuration.Interfaces;
using FluentFTP;
using MimeDetective.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PartyMaker.Story.Image
{
    public class WebAppGetImageStory : IStory<WebAppGetImageStoryContext, (MemoryStream stream, string mime)>
    {
        private readonly IRepository _repository;
        private readonly IImageStoreSetting _imageStoreSettings;
        private readonly IAppLogger _appLogger;
        private readonly FtpClient _ftpClient;

        public WebAppGetImageStory(
            IRepository repository,
            IImageStoreSetting imageStoreSettings,
            IAppLogger appLogger)
        {
            _repository = repository;
            _imageStoreSettings = imageStoreSettings;
            _appLogger = appLogger;
            _ftpClient = new FtpClient(_imageStoreSettings.Url)
            {
                Credentials = new System.Net.NetworkCredential(_imageStoreSettings.Username, _imageStoreSettings.Password)
            };
        }

        public (MemoryStream stream, string mime) Execute(WebAppGetImageStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<(MemoryStream stream, string mime)> ExecuteAsync(WebAppGetImageStoryContext context)
        {
            _appLogger.Info($"Start getting image file with id - {context.Id}");
            try
            {
                await _ftpClient.ConnectAsync();
                var image = _repository.Get<Domain.Entities.Image>(t => t.Id == context.Id);
                if (image == null)
                {
                    _appLogger.Error($"Get error {WebAppErrors.ImageNotFound}");
                    return (null, string.Empty);
                }

                byte[] file = await _ftpClient.DownloadAsync(image.RealPath, 0);
                var stream = new MemoryStream(file);
                _appLogger.Info($"Start read file {stream.Length}");
                var fileType = stream.GetFileType();

                return (stream, fileType.Mime);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex);
                await _ftpClient.DisconnectAsync();
                return (null, string.Empty);
            }
        }
    }
}

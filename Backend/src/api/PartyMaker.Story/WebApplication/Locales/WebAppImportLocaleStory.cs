using bgTeam;
using bgTeam.DataAccess;
using FluentFTP;
using PartyMaker.Common;
using PartyMaker.Common.Impl;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Domain.Entities;
using PartyMaker.Dto.WebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace PartyMaker.Story.WebApplication.Locales
{
    class WebAppImportLocaleStory : IStory<WebAppImportLocaleStoryContext, PartyMaker.Dto.WebApp.WebAppResponseDto>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IEncodingDetectionService _encodingDetectionService;
        private readonly ICrudService _crudService;
        private readonly IRepository _repository;
        private readonly IAppLogger _appLogger;
		private readonly FtpClient _ftpClient;

		public WebAppImportLocaleStory(
			IConnectionFactory connectionFactory,
			IImageStoreSetting imageStoreSettings,
			IEncodingDetectionService encodingDetectionService,
			ICrudService crudService,
			IRepository repository,
			IAppLogger appLogger)
        {
            _connectionFactory = connectionFactory;
            _encodingDetectionService = encodingDetectionService;
            _crudService = crudService;
            _repository = repository;
            _appLogger = appLogger;
			_ftpClient = new FtpClient(imageStoreSettings.Url)
			{
				Credentials = new System.Net.NetworkCredential(imageStoreSettings.Username, imageStoreSettings.Password)
			};
		}

        public WebAppResponseDto Execute(WebAppImportLocaleStoryContext context)
        {
			return ExecuteAsync(context).Result;
        }

        public async Task<WebAppResponseDto> ExecuteAsync(WebAppImportLocaleStoryContext context)
		{

			using var dbConnection = await _connectionFactory.CreateAsync();
			var locale = await _repository.GetAsync<Localization>(t => !t.IsDeleted && t.Id == context.LocaleId, dbConnection);
			if (locale == null)
			{
				_appLogger.Warning(string.Format("Cannot import data for non exists locale {0}", context.LocaleId));
				return new WebAppResponseDto { IsSuccess = false, IsNotFound = true };
			}

			var importedItems = await ParseFileAsync(context.FileId, dbConnection);
			if (importedItems == null)
			{
				_appLogger.Warning(string.Format("Localization data in blob {0} is broken", context.FileId));
				return new WebAppResponseDto { IsSuccess = false, IsNotFound = true };
			}

			var localesItem = new List<LocalizationItem>();

			foreach (var itm in importedItems)
            {
				var localizationItem = new LocalizationItem
				{
					Key = itm.Key,
					Value = itm.Value,
					LocalizationId = locale.Id
				};
				localizationItem.MarkAsNew();
				await _crudService.InsertAsync(localizationItem, dbConnection);
			}

			return new WebAppResponseDto { IsSuccess = true };
		}

		private async Task<IReadOnlyCollection<LocalizationItem>> ParseFileAsync(Guid blobId, IDbConnection dbConnection)
		{
			var csvParserOptions = new CsvParserOptions(false, ',');
			var csvMapper = new LocalizationItemMapping();
			var csvParser = new CsvParser<LocalizationItem>(csvParserOptions, csvMapper);
			var csvReaderOptions = new CsvReaderOptions(new[] { "\r\n", "\n" });

			var file = await _repository.GetAsync<Domain.Entities.Image>(t => t.Id == blobId);
			byte[] fileContent = await _ftpClient.DownloadAsync(file.RealPath, 0);

			var encodingInfo = _encodingDetectionService.Detect(fileContent) ?? new EncodingInfo(System.Text.Encoding.UTF8, 0);
			var fileData = encodingInfo.Encoding.GetString(fileContent, encodingInfo.BomSize, fileContent.Length - encodingInfo.BomSize);

			if (string.IsNullOrEmpty(fileData))
				return null;

			var values = csvParser
				.ReadFromString(csvReaderOptions, fileData)
				.ToList();

			if (values.Any(x => !x.IsValid)
				|| values.Any(x => string.IsNullOrEmpty(x.Result.Key)))
				return null;

			return values
				.Select(value => value.Result)
				.ToArray();
		}

		private class LocalizationItemMapping : CsvMapping<LocalizationItem>
        {
            public LocalizationItemMapping()
            {
                MapProperty(0, x => x.Key);
                MapProperty(1, x => x.Value);
            }
        }
    }
}

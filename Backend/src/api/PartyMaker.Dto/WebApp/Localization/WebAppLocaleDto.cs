using System;
using System.Collections.Generic;

namespace PartyMaker.Dto.WebApp.Localization
{
    public class WebAppLocaleDto : WebAppEntityDto
    {
		public string Name { get; set; }

		public IEnumerable<WebAppLocaleItemDto> Items { get; set; }

		public WebAppLocaleDto()
		{
			Items = new WebAppLocaleItemDto[0];
		}
	}
}

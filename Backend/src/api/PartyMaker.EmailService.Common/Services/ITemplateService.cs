using System.Collections.Generic;

namespace PartyMaker.EmailService.Common.Services
{
    public interface ITemplateService
    {
        string CreateFromTemplate(string templateName, Dictionary<string, object> parameters);

        string GetTemplateDir(string templateName);
    }
}

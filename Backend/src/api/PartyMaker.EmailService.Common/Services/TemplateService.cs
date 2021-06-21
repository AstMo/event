using Antlr4.StringTemplate;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PartyMaker.EmailService.Common.Services
{
    public class TemplateService : ITemplateService
    {
        public enum EEmailBodyType
        {
            Plain,
            Html
        }

        private const string TemplateDir = "Content/Templates";

        private readonly string _templateDirectoryRoot;

        public TemplateService()
        {
            _templateDirectoryRoot = $"{Path.GetDirectoryName(Assembly.GetAssembly(typeof(TemplateService)).Location)}/{TemplateDir}";

            if (!Directory.Exists(_templateDirectoryRoot))
                Directory.CreateDirectory(_templateDirectoryRoot);
        }

        public string CreateFromTemplate(
            string templateName,
            Dictionary<string, object> parameters)
        {
            var path = Path.Combine(_templateDirectoryRoot, templateName);
            var template = new Template(File.ReadAllText(path), '$', '$');

            foreach (var param in parameters)
            {
                template.Add(param.Key, param.Value);
            }

            return template.Render();
        }

        public string GetTemplateDir(string templateName)
        {
            return Path.Combine(_templateDirectoryRoot, templateName);
        }
    }
}

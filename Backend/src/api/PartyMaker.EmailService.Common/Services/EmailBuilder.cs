using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

namespace PartyMaker.EmailService.Common.Services
{
    class EmailBuilder
    {
        private const string ImmageTagEx = @"(?<prefix><img[^>]*?src\s*=\s*[""']?)(?<src>[^'"" >]+?)(?<suffix>[ '""][^>]*?>)";

        private readonly string _baseDir;
        private readonly MailMessage _message;
        private readonly Dictionary<string, string> _imageMap;

        private bool _hasHtmlPart;

        public EmailBuilder(
            string from,
            string to,
            string subject,
            string baseDir)
        {
            _baseDir = baseDir;
            _message = new MailMessage(from, to) { Subject = subject };
            _imageMap = new Dictionary<string, string>();
        }

        public EmailBuilder WithPlainBody(string content)
        {
            var plainPart = AlternateView.CreateAlternateViewFromString(
                content,
                null,
                MediaTypeNames.Text.Plain);

            _message.AlternateViews.Add(plainPart);

            return this;
        }

        public EmailBuilder WithHtmlBody(string content)
        {
            _hasHtmlPart = !string.IsNullOrEmpty(content);
            _imageMap.Clear();
            content = EmbededImages(content);

            var htmlPart = AlternateView.CreateAlternateViewFromString(
                content,
                null,
                MediaTypeNames.Text.Html);

            foreach (var image in _imageMap)
            {
                var resource = new LinkedResource(Path.Combine(_baseDir, image.Key), MediaTypeNames.Image.Jpeg) { ContentId = image.Value };
                htmlPart.LinkedResources.Add(resource);
            }

            _message.AlternateViews.Add(htmlPart);

            return this;
        }

        public EmailBuilder WithAttachment(Stream data, string filename, string contentType)
        {
            var attachment = new Attachment(data, new ContentType(contentType));
            attachment.ContentDisposition.FileName = filename;
            attachment.TransferEncoding = TransferEncoding.Base64;

            _message.Attachments.Add(attachment);

            return this;
        }

        public MailMessage Build()
        {
            _message.IsBodyHtml = _hasHtmlPart;
            return _message;
        }

        private string EmbededImages(string bodyContent)
        {
            bodyContent = Regex.Replace(bodyContent, ImmageTagEx, ReplaceWithContentId, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return bodyContent;
        }

        private string ReplaceWithContentId(Match m)
        {
            var prefix = m.Groups["prefix"].Value;
            var originalSource = m.Groups["src"].Value;
            var suffix = m.Groups["suffix"].Value;
            if (!_imageMap.ContainsKey(originalSource))
                _imageMap.Add(originalSource, string.Format("image_{0}", _imageMap.Count));

            return string.Format("{0}cid:{1}{2}", prefix, _imageMap[originalSource], suffix);
        }
    }
}

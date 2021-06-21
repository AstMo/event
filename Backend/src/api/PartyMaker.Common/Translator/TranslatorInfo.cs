using System;

namespace PartyMaker.Common.Translator
{
    public class TranslatorInfo
    {
        public Type SourceType { get; set; }

        public Type DestinationType { get; set; }

        public ITranslator Translator { get; set; }
    }
}

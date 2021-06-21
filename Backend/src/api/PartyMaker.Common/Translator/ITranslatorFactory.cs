using System;

namespace PartyMaker.Common.Translator
{
    public interface ITranslatorFactory
    {
        void Initialize();

        ITranslator<TSource, TDestination> GetTranslator<TSource, TDestination>();

        ITranslator GetTranslator(Type sourceType, Type destinationType);
    }
}

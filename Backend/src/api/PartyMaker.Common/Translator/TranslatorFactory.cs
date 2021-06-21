using AutoMapper;
using PartyMaker.Common.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyMaker.Common.Translator
{
    public abstract class TranslatorFactory : ITranslatorFactory
    {
        private readonly IServiceProvider _kernel;

        private readonly List<TranslatorInfo> _translators;

        private bool _initialized;

        protected TranslatorFactory(IServiceProvider kernel)
        {
            _kernel = kernel;
            _translators = new List<TranslatorInfo>();
        }

        protected IMapper Mapper { get; private set; }

        protected MapperConfiguration MapperConfiguration { get; private set; }

        public void Initialize()
        {
            MapperConfiguration = new MapperConfiguration(RegisterTranslators);
            Mapper = MapperConfiguration.CreateMapper();
            MapperConfiguration.AssertConfigurationIsValid();

            _initialized = true;
        }

        public ITranslator<TSource, TDestination> GetTranslator<TSource, TDestination>()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "Factory does not initialized. Ensure that method Initialize called before any other invokations");
            }

            return (ITranslator<TSource, TDestination>)_translators
                .First(x => x.SourceType == typeof(TSource) && x.DestinationType == typeof(TDestination))
                .Translator;
        }

        public ITranslator GetTranslator(Type sourceType, Type destinationType)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "Factory does not initialized. Ensure that method Initialize called before any other invokations");
            }

            return _translators
                .First(x => x.SourceType == sourceType &&
                            x.DestinationType == destinationType)
                .Translator;
        }

        protected abstract void RegisterTranslators(IMapperConfigurationExpression configurationExpression);

        protected void RegisterTranslator<TSource, TDestination>(ITranslator<TSource, TDestination> translator)
        {
            translator.Configure();

            _translators.Add(new TranslatorInfo
            {
                SourceType = typeof(TSource),
                DestinationType = typeof(TDestination),
                Translator = translator,
            });
        }

        protected void RegisterTranslator<TTranslator, TSource, TDestination>(
            IMapperConfigurationExpression configurationExpression)
            where TTranslator : ITranslator<TSource, TDestination>
        {
            var translator = InstantinateTranslator<TSource, TDestination, TTranslator>(configurationExpression);
            translator.Configure();

            _translators.Add(new TranslatorInfo
            {
                SourceType = typeof(TSource),
                DestinationType = typeof(TDestination),
                Translator = translator,
            });
        }

        protected ITranslator<TSource, TDestination> InstantinateTranslator<TSource, TDestination, TTranslator>(
            IMapperConfigurationExpression configurationExpression)
            where TTranslator : ITranslator<TSource, TDestination>
        {
            try
            {
                return (ITranslator<TSource, TDestination>)_kernel.RuntimeResolve(
                    typeof(TTranslator),
                    new NamedParameter("configurationExpression", configurationExpression),
                    new NamedParameter("mapper", new Lazy<IMapper>(() => Mapper)));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"Cannot create translator of type: {typeof(TTranslator)}. Check constructor parameters constraint!",
                    e);
            }
        }
    }
}

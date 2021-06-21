using AutoMapper;
using System;

namespace PartyMaker.Common.Translator
{
    public class AutomapperTranslator<TSource, TDestination> : ITranslator<TSource, TDestination>
    {
        private readonly Lazy<IMapper> _mapper;

        private readonly IMappingExpression<TSource, TDestination> _mapping;

        public AutomapperTranslator(IMapperConfigurationExpression configurationExpression, Lazy<IMapper> mapper)
        {
            _mapper = mapper;
            _mapping = configurationExpression.CreateMap<TSource, TDestination>();
        }

        protected IMapper Mapper => _mapper.Value;

        protected IMappingExpression<TSource, TDestination> Mapping => _mapping;

        public virtual void Configure()
        {
            _mapping
                .BeforeMap(BeforeMap)
                .AfterMap(AfterMap);
        }

        public TDestination Translate(TSource source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public object Translate(object source)
        {
            return Translate((TSource)source);
        }

        public void Update(TSource source, TDestination destination)
        {
            Mapper.Map(source, destination);
        }

        public void Update(object source, object destination)
        {
            Mapper.Map((TSource)source, (TDestination)destination);
        }

        protected virtual void BeforeMap(TSource source, TDestination destintion)
        {
        }

        protected virtual void AfterMap(TSource source, TDestination destination)
        {
        }
    }
}

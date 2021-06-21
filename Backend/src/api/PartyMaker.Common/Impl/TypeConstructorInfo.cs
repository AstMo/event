using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PartyMaker.Common.Impl
{
    public class TypeConstructorInfo
    {
        public TypeConstructorInfo(Type type)
        {
            var parametersInfo = new List<ParameterInfo>();

            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var ctor = constructors.FirstOrDefault();

            if (ctor == null)
            {
                throw new TypeLoadException($"Failed to find constructor for type {type.FullName}");
            }

            foreach (var parameterInfo in ctor.GetParameters())
            {
                parametersInfo.Add(parameterInfo);
            }

            Parameters = parametersInfo;
            Ctor = ctor;
            Type = type;
        }

        public ConstructorInfo Ctor { get; }

        public ICollection<ParameterInfo> Parameters { get; }

        public Type Type { get; }
    }
}

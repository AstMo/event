using PartyMaker.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PartyMaker.Common.Impl
{
    public static class ServiceDiscoveryExtension
    {
        private static readonly Dictionary<Type, TypeConstructorInfo> CtorInfo = new Dictionary<Type, TypeConstructorInfo>();
        private static readonly object CtorInfoLock = new object();

        public static T RuntimeResolve<T>(this IServiceProvider kernel, params NamedParameter[] parameters)
            where T : class
        {
            return (T)kernel.RuntimeResolve(typeof(T), parameters);
        }

        public static object RuntimeResolve(this IServiceProvider kernel, Type type, params NamedParameter[] parameters)
        {
            if (kernel.GetService(type) != null)
            {
                return kernel.GetService(type);
            }

            bool cached;
            TypeConstructorInfo ctorInfo;
            lock (CtorInfoLock)
            {
                cached = CtorInfo.TryGetValue(type, out ctorInfo);
            }

            if (!cached)
            {
                ctorInfo = new TypeConstructorInfo(type);
                lock (CtorInfoLock)
                {
                    CtorInfo.Add(type, ctorInfo);
                }
            }

            var arguments = CreateParameters(kernel, ctorInfo, parameters);
            try
            {
                var result = ctorInfo.Ctor.Invoke(arguments.ToArray());
                return result;
            }
            catch (TargetInvocationException invocation)
            {
                throw new ServiceDiscoveryException($"Failed to construct {type.Name}", invocation);
            }
        }

        private static List<object> CreateParameters(IServiceProvider kernel, TypeConstructorInfo ctorInfo, NamedParameter[] userParameters)
        {
            var arguments = new List<object>();

            foreach (var parameterInfo in ctorInfo.Parameters)
            {
                object parameterValue;
                var parameter = userParameters.FirstOrDefault(x => x.Name == parameterInfo.Name);

                if (parameter == null)
                {
                    parameterValue = kernel.GetService(parameterInfo.ParameterType);
                    if (parameterValue == null)
                    {
                        if (parameterInfo.IsOptional)
                        {
                            parameterValue = Type.Missing;
                        }
                        else
                        {
                            throw new ServiceDiscoveryException(
                                $"Failed to resolve parameter for parameter {parameterInfo.Name} of type {parameterInfo.ParameterType.Name} when creating {ctorInfo.Type.FullName}");
                        }
                    }
                }
                else
                {
                    parameterValue = parameter.Value;
                }

                arguments.Add(parameterValue);
            }

            return arguments;
        }
    }
}

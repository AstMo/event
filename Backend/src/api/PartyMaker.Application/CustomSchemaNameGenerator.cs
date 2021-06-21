using NJsonSchema.Generation;
using System;
using System.Linq;

namespace PartyMaker.Application
{
    public class CustomSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return ConstructSchemaId(type);
        }

        public string ConstructSchemaId(Type type)
        {
            var typeName = type.Name;
            if (type.IsGenericType)
            {
                var genericArgs = string.Join(", ", type.GetGenericArguments().Select(ConstructSchemaId));

                int index = typeName.IndexOf('`');
                var typeNameWithoutGenericArity = index == -1 ? typeName : typeName.Substring(0, index);

                return $"{typeNameWithoutGenericArity}<{genericArgs}>";
            }
            return typeName;
        }
    }
}

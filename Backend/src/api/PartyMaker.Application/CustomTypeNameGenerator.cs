using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Application
{
    public class CustomTypeNameGenerator : DefaultTypeNameGenerator
    {
        /// <inheritdoc />
        public override string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            if (string.IsNullOrEmpty(typeNameHint) && !string.IsNullOrEmpty(schema.DocumentPath))
            {
                typeNameHint = schema.DocumentPath.Replace("\\", "/").Split('/').Last();
            }

            return typeNameHint;
        }
    }
}
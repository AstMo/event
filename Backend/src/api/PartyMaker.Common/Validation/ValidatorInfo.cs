using FluentValidation;
using System;

namespace PartyMaker.Common.Validation
{
    public class ValidatorInfo
    {
        public Type Type { get; set; }

        public IValidator Validator { get; set; }
    }
}

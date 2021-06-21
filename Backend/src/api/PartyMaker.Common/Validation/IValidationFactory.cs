using FluentValidation;
using System;

namespace PartyMaker.Common.Validation
{
    public interface IValidationFactory
    {
        void Initialize();

        IValidator<T> GetValidator<T>();

        IValidator GetValidator(Type type);
    }
}

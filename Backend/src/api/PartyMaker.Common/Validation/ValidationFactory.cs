using PartyMaker.Common.Exceptions;
using PartyMaker.Common.Impl;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyMaker.Common.Validation
{
    public abstract class ValidationFactory : IValidationFactory
    {
        private readonly IServiceProvider _kernel;
        private readonly List<ValidatorInfo> _validators;
        private bool _initialized;

        protected ValidationFactory(IServiceProvider kernel)
        {
            _kernel = kernel;
            _validators = new List<ValidatorInfo>();
        }

        public void Initialize()
        {
            RegisterValidators();
            _initialized = true;
        }

        public IValidator<T> GetValidator<T>()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "Factory does not initialized. Ensure that method Initialize called before any other invokations");
            }

            var validator = (IValidator<T>)_validators
                .FirstOrDefault(x => x.Type == typeof(T))?
                .Validator;
            if (validator == null)
            {
                throw new ServiceDiscoveryException(
                    string.Format(
                        "Validator for type {0} was not found. Factory contains validators for {1} types: {2}",
                        typeof(T).Name,
                        _validators.Count,
                        string.Join(",", _validators.Select(x => x.Type.Name).OrderBy(x => x).ToArray())));
            }

            return validator;
        }

        public IValidator GetValidator(Type type)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "Factory does not initialized. Ensure that method Initialize called before any other invokations");
            }

            var validator = _validators
                .First(x => x.Type == type)
                .Validator;

            if (validator == null)
            {
                throw new ServiceDiscoveryException(
                    string.Format(
                        "Validator for type {0} was not found. Factory contains validators for {1} types: {2}",
                        type.Name,
                        _validators.Count,
                        string.Join(",", _validators.Select(x => x.Type.Name).OrderBy(x => x).ToArray())));
            }

            return validator;
        }

        protected abstract void RegisterValidators();

        protected void RegisterValidator<TValidator, T>()
            where TValidator : IValidator<T>
        {
            var validator = InstantinateValidator<T, TValidator>();

            _validators.Add(new ValidatorInfo
            {
                Type = typeof(T),
                Validator = validator,
            });
        }

        protected IValidator<T> InstantinateValidator<T, TValidator>()
            where TValidator : IValidator<T>
        {
            try
            {
                return (IValidator<T>)_kernel.RuntimeResolve(typeof(TValidator));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"Cannot create validator of type: {typeof(TValidator)}. Check constructor parameters constraint!",
                    e);
            }
        }
    }
}

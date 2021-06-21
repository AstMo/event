using PartyMaker.Common.Exceptions;
using PartyMaker.Common.Impl;
using PartyMaker.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartyMaker.Common.Approver
{
    public abstract class ApproverFactory : IApproverFactory
    {
        private readonly IServiceProvider _kernel;
        private readonly List<RequestApproverInfo> _approvers;
        private bool _initialized;

        protected ApproverFactory(IServiceProvider kernel)
        {
            _kernel = kernel;
            _approvers = new List<RequestApproverInfo>();
        }

        public void Initialize()
        {
            RegisterApprovers();
            _initialized = true;
        }

        public IRequestApprover<T> GetApprover<T>()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException(
                    "Factory does not initialized. Ensure that method Initialize called before any other invokations");
            }

            var validator = (IRequestApprover<T>)_approvers
                .FirstOrDefault(x => x.Type == typeof(T))?
                .Approver;
            if (validator == null)
            {
                throw new ServiceDiscoveryException(
                    string.Format(
                        "Validator for type {0} was not found. Factory contains validators for {1} types: {2}",
                        typeof(T).Name,
                        _approvers.Count,
                        string.Join(",", _approvers.Select(x => x.Type.Name).OrderBy(x => x).ToArray())));
            }

            return validator;
        }

   

        protected abstract void RegisterApprovers();

        protected void RegisterApprover<TApprover, TRequest>()
            where TApprover : IRequestApprover<TRequest>
        {
            var approver = InstantinateApprover<TRequest, TApprover>();

            _approvers.Add(new RequestApproverInfo
            {
                Type = typeof(TRequest),
                Approver = approver,
            });
        }

        protected IRequestApprover<T> InstantinateApprover<T, TValidator>()
            where TValidator : IRequestApprover<T>
        {
            try
            {
                return (IRequestApprover<T>)_kernel.RuntimeResolve(typeof(TValidator));
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

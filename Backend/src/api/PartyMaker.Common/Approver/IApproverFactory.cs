using System;

namespace PartyMaker.Common.Approver
{
    public interface IApproverFactory
    {
        void Initialize();

        IRequestApprover<T> GetApprover<T>();
    }
}

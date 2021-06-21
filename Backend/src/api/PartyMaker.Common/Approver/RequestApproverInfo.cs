using FluentValidation;
using System;

namespace PartyMaker.Common.Approver
{
    class RequestApproverInfo
    {
        public Type Type { get; set; }

        public IValidator Approver { get; set; }
    }
}

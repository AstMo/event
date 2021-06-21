using FluentValidation;

namespace PartyMaker.Common.Approver
{
    public class RequestApprover<TRequest>: AbstractValidator<TRequest>, IRequestApprover<TRequest>
    {
    }
}

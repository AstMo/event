using FluentValidation;

namespace PartyMaker.Common.Approver
{
    public interface IRequestApprover<TRequest>: IValidator<TRequest>
    {
    }
}

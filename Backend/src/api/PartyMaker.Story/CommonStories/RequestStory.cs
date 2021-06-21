using bgTeam;
using PartyMaker.Common.Approver;
using PartyMaker.Common.Request;
using PartyMaker.Common.Validation;
using PartyMaker.Dto.WebApp;
using System.Linq;
using System.Threading.Tasks;

namespace PartyMaker.Story.CommonStories
{
    public abstract class RequestStory<TStoryContext, TReturnValue>
        where TStoryContext : IRequest
        where TReturnValue: WebAppResponseDto, new()
    {
        private readonly IWebApproverFactory _webApproverFactory;
        private readonly IWebAppValidatorFactory _webAppValidatorFactory;
        private readonly IAppLogger _appLogger;

        public RequestStory(
            IWebApproverFactory webApproverFactory,
            IWebAppValidatorFactory webAppValidatorFactory,
            IAppLogger appLogger)
        {
            _webApproverFactory = webApproverFactory;
            _webAppValidatorFactory = webAppValidatorFactory;
            _appLogger = appLogger;
        }

        public TReturnValue Execute(TStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }


        public async Task<TReturnValue> ExecuteAsync(TStoryContext context)
        {
            _appLogger.Info($"Start base request story");

            var validator = _webAppValidatorFactory.GetValidator<TStoryContext>();
            var approver = _webApproverFactory.GetApprover<TStoryContext>();
            var approveResult = await approver.ValidateAsync(context);

            if (!approveResult.IsValid)
            {
                string message = string.Empty;
                foreach (var failure in approveResult.Errors)
                {
                    if (string.IsNullOrEmpty(failure.PropertyName))
                    {
                        message += $"{failure.ErrorMessage}&";
                    }
                }
                _appLogger.Error($"Approve not correct with error {message}");
                return new TReturnValue
                {
                    IsInvalid = true,
                    IsSuccess = false,
                    Message = message,
                    Errors = approveResult.Errors.Select(t => new WebAppErrorDto
                    {
                        FieldName = t.PropertyName,
                        Message = t.ErrorMessage
                    }),
                };
            }

            var validationResult = await validator.ValidateAsync(context);
            if (!validationResult.IsValid)
            {
                string message = string.Empty;
                foreach (var failure in validationResult.Errors)
                {
                    if (string.IsNullOrEmpty(failure.PropertyName))
                    {
                        message += $"{failure.ErrorMessage}&";
                    }
                }
                _appLogger.Error($"Validation not correct with error {message}");
                return new TReturnValue
                {
                    IsInvalid = true,
                    IsSuccess = false,
                    Message = message,
                    Errors = validationResult.Errors.Select(t => new WebAppErrorDto
                    {
                        FieldName = t.PropertyName,
                        Message = t.ErrorMessage
                    }),
                };
            }

            return await Run(context);
        }

        protected abstract Task<TReturnValue> Run(TStoryContext context);
    }
}
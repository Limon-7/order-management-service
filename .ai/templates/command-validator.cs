using {{RootNamespace}}.Application.Commands.[ContextName];
using {{RootNamespace}}.Shared.Common.Constants;
using FluentValidation;

namespace {{RootNamespace}}.Application.CommandHandlers.Validators;

/// <summary>
/// Template for a CQRS Command Validator.
/// It must extend AbstractCommandValidator&lt;TCommand&gt; and use FluentValidation.
/// Failing validations should pass BusinessViolationCodes to the client via state.
/// </summary>
public class [ActionName]CommandValidator : AbstractCommandValidator<[ActionName]Command>
{
    public [ActionName]CommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            // Use GetEventMessage to attach specific violation codes for the client
            .WithState(x => GetEventMessage(BusinessViolationCodes.[SpecificErrorCode], nameof(x.ResourceId)))
            .WithMessage("Resource ID is required");
            
        // Example of conditional validation
        // When(x => x.IsActive, () => 
        // {
        //     RuleFor(x => x.StartDate)
        //         .NotEmpty()
        //         .WithState(x => GetEventMessage("ErrorCode", "StartDate"))
        //         .WithMessage("Start Date is required when active");
        // });
    }
}

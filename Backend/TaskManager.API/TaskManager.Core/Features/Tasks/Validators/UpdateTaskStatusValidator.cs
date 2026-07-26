using FluentValidation;
using TaskManager.Core.Features.Tasks.DTOs;

namespace TaskManager.Core.Features.Tasks.Validators;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusDto>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid task status.");
    }
}

using FluentValidation;
using TaskManager.Core.Features.Tasks.DTOs;

namespace TaskManager.Core.Features.Tasks.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(150).WithMessage("Task title must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid task status.");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0).WithMessage("A valid ProjectId is required.");
            
        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(System.DateTime.Today).When(x => x.DueDate.HasValue)
            .WithMessage("Due date cannot be in the past.");
    }
}

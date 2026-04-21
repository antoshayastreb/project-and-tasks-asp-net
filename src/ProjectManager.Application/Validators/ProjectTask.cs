using FluentValidation;
using ProjectManager.Application.DTOs.ProjectTask;

namespace ProjectManager.Application.Validators.Project;

/// <summary>
/// Набор правил валидации для создания задачи.
/// </summary>
public class CreateProjectTaskDTOValidator : AbstractValidator<CreateProjectTaskDto>
{
    public CreateProjectTaskDTOValidator()
    {
        //Правило для названия
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Is required")
            .Length(3, 250)
            .WithMessage("Must have length between 3 and 250 characters");
        //Правило для описания
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Must have maximum length of 20000 characters");
    }
}

/// <summary>
/// Набор правил валидации для обновления задачи.
/// </summary>
public class UpdateProjectTaskDtoValidator : AbstractValidator<UpdateProjectTaskDto>
{
    public UpdateProjectTaskDtoValidator()
    {
        //Правило для названия
        RuleFor(x => x.Title)
            .Length(3, 250)
            .WithMessage("Must have length between 3 and 250 characters")
            .When(x => x.Title is not null);

        //Правило для описания
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Must have maximum length of 20000 characters")
            .When(x => x.Description is not null);
    }
}



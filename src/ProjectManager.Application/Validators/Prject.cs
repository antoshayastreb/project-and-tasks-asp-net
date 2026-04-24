using FluentValidation;
using ProjectManager.Application.DTOs.Project;

namespace ProjectManager.Application.Validators.Project;

/// <summary>
/// Набор правил валидации для создания проекта.
/// </summary>
public class CreateProjectDTOValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDTOValidator()
    {
        //Правило для наименования
        RuleFor(x => x.Name)
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
/// Набор правил валидации для обновления проекта.
/// </summary>
public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        //Правило для наименования
        RuleFor(x => x.Name)
            .Length(3, 250)
            .WithMessage("Must have length between 3 and 250 characters")
            .When(x => x.Name is not null);

        //Правило для описания
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Must have maximum length of 20000 characters")
            .When(x => x.Description is not null);
    }
}



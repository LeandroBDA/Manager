using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators;

public class Uservalidator : AbstractValidator<User>
{
    public Uservalidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("A entidade não pode ser vazia.")

            .NotNull()
            .WithMessage("A entidade nâo pode ser nula.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome não pode ser nulo.")

            .NotNull()
            .WithMessage("O nome não pode ser vazio.")

            .MinimumLength(3)
            .WithMessage("O nome deve ter no mínimo 3 caracteres.")

            .MaximumLength(80)
            .WithMessage("O nome deve ter no máximo 80 caracteres.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha não pode ser nula.")

            .NotNull()
            .WithMessage("A senha não pode ser vazia.")

            .MinimumLength(8)
            .WithMessage("A senha deve ter no mínimo 8 caracteres.")

            .MaximumLength(30)
            .WithMessage("A senha deve ter no máximo 30 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email não pode ser nulo.")

            .NotNull()
            .WithMessage("O email não pode ser vazio.")

            .Length(10, 180)
            .WithMessage("O Email informado deve ter no mínimo 10 caracteres e no máximo 180 caracteres")

            .EmailAddress()
            .WithMessage("O email informado não é válido");
    }
}
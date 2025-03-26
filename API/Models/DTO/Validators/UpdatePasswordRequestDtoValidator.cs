using FluentValidation;

namespace API.Models.DTO.Validators;

public class UpdatePasswordRequestDtoValidator : AbstractValidator<UpdatePasswordRequestDto>
{
    public UpdatePasswordRequestDtoValidator()
    {   
        RuleFor( input => input.UserId)
            .NotEmpty().WithMessage("Il campo 'UserId' non può essere vuoto");

        RuleFor( input => input.PasswordCorrente)
            .NotEmpty().WithMessage("Il campo 'PasswordCorrente' è obbligatorio")
            .MinimumLength(8).WithMessage("La password deve essere lunga almeno 8 caratteri")
            .Matches(@"[A-Z]").WithMessage("La password deve contenere almeno una lettera maiuscola")
            .Matches(@"[a-z]").WithMessage("La password deve contenere almeno una lettera minuscola")
            .Matches(@"[0-9]").WithMessage("La password deve contenere almeno un numero")
            .Matches(@"[\W_]").WithMessage("La password deve contenere almeno un carattere speciale (come @, #, $, etc.)");


        RuleFor( input => input.NuovaPassword )
            .NotEmpty().WithMessage("Il campo 'PasswordCorrente' è obbligatorio")
            .MinimumLength(8).WithMessage("La password deve essere lunga almeno 8 caratteri")
            .Matches(@"[A-Z]").WithMessage("La password deve contenere almeno una lettera maiuscola")
            .Matches(@"[a-z]").WithMessage("La password deve contenere almeno una lettera minuscola")
            .Matches(@"[0-9]").WithMessage("La password deve contenere almeno un numero")
            .Matches(@"[\W_]").WithMessage("La password deve contenere almeno un carattere speciale (come @, #, $, etc.)");
    }   
}

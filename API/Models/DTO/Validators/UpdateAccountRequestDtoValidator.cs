using FluentValidation;

namespace API.Models.DTO.Validators;

public class UpdateAccountRequestDtoValidator : AbstractValidator<UpdateAccountRequestDto>
{
    public UpdateAccountRequestDtoValidator()
    {
        RuleFor( account => account.UserId)
            .NotEmpty()
            .WithMessage("Il campo 'UserId' non può essere vuoto");

        RuleFor( account => account.Nome)
            .NotEmpty()
            .WithMessage("Il campo 'Nome' non può essere vuoto")
            .MinimumLength(3).WithMessage("Il campo 'FirstName' deve contenere almeno 3 caratteri")
            .MaximumLength(50).WithMessage("Il campo 'FirstName' deve contenere al massimo 50 caratteri");


        RuleFor( account => account.Cognome)
            .NotEmpty()
            .WithMessage("Il campo 'Cognome' non può essere vuoto")
            .MinimumLength(3).WithMessage("Il campo 'LastName' deve contenere almeno 3 caratteri")
            .MaximumLength(50).WithMessage("Il campo 'LastName' deve contenere al massimo 50 caratteri");   

        
        RuleFor( account => account.Email)
            .NotEmpty()
            .WithMessage("Il campo 'Email' non può essere vuoto")
            .EmailAddress().WithMessage("Il campo 'Email' non è un indirizzo email valido");


    }
}

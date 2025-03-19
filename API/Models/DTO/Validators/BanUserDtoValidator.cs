using FluentValidation;

namespace API.Models.DTO.Validators;

public class BanUserDtoValidator : AbstractValidator<BanUserDto>
{
    public BanUserDtoValidator()
    {
        RuleFor( input => input.Email )
            .NotEmpty().WithMessage("Il campo 'Nome' non può essere vuoto")
            .EmailAddress().WithMessage("Il campo 'Email' deve essere un indirizzo valido");
        
        RuleFor( input => input.Motivazione )
            .NotEmpty().WithMessage("Il campo 'Motivazione' non può essere vuoto");
            
    }
}

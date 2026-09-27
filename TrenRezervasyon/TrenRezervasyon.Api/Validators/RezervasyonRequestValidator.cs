using FluentValidation;
using TrenRezervasyon.Api.DTOs.Requests;

namespace TrenRezervasyon.Api.Validators;

public class RezervasyonRequestValidator
    : AbstractValidator<RezervasyonRequestDto>
{
    public RezervasyonRequestValidator(IValidator<TrenDto> trenValidator)
    {
        RuleFor(x => x.RezervasyonYapilacakKisiSayisi)
            .GreaterThan(0)
            .WithMessage("Rezervasyon kişi sayısı sıfırdan büyük olmalıdır.");

        RuleFor(x => x.Tren)
            .NotNull()
            .WithMessage("Tren bilgisi zorunludur.")
            .SetValidator(trenValidator);
    }
}

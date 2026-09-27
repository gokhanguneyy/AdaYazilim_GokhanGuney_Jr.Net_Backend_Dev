using FluentValidation;
using TrenRezervasyon.Api.DTOs.Requests;

namespace TrenRezervasyon.Api.Validators;

public class VagonDtoValidator : AbstractValidator<VagonDto>
{
    public VagonDtoValidator()
    {
        RuleFor(x => x.Ad)
            .NotEmpty()
            .WithMessage("Vagon adı boş olamaz.");

        RuleFor(x => x.Kapasite)
            .GreaterThan(0)
            .WithMessage("Vagon kapasitesi sıfırdan büyük olmalıdır.");

        RuleFor(x => x.DoluKoltukAdet)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Dolu koltuk sayısı negatif olamaz.")
            .LessThanOrEqualTo(x => x.Kapasite)
            .WithMessage("Dolu koltuk sayısı kapasiteyi aşamaz.");
    }
}
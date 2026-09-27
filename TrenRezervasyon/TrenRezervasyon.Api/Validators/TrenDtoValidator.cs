using FluentValidation;
using TrenRezervasyon.Api.DTOs.Requests;

namespace TrenRezervasyon.Api.Validators;

public class TrenDtoValidator : AbstractValidator<TrenDto>
{
    public TrenDtoValidator(IValidator<VagonDto> vagonValidator)
    {
        RuleFor(x => x.Ad)
            .NotEmpty()
            .WithMessage("Tren adı boş olamaz.");

        RuleFor(x => x.Vagonlar)
            .NotEmpty()
            .WithMessage("Trende en az bir vagon bulunmalıdır.");

        RuleForEach(x => x.Vagonlar)
            .NotNull()
            .WithMessage("Vagon bilgisi null olamaz.")
            .SetValidator(vagonValidator);
    }
}
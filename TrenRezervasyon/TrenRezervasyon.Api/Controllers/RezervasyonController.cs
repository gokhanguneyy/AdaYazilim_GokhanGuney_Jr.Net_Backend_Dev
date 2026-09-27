using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TrenRezervasyon.Api.DTOs.Requests;
using TrenRezervasyon.Api.DTOs.Responses;
using TrenRezervasyon.Api.Services;

namespace TrenRezervasyon.Api.Controllers;

[ApiController]
[Route("api/rezervasyon")]
public class RezervasyonController : ControllerBase
{
    private readonly IRezervasyonService _rezervasyonService;
    private readonly IValidator<RezervasyonRequestDto> _validator;

    public RezervasyonController(IRezervasyonService rezervasyonService, IValidator<RezervasyonRequestDto> validator)
    {
        _rezervasyonService = rezervasyonService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<RezervasyonResponseDto>> Hesapla([FromBody] RezervasyonRequestDto request, CancellationToken cancellationToken)
    {
        var dogrulamaSonucu = await _validator.ValidateAsync(request, cancellationToken);

        if (!dogrulamaSonucu.IsValid)
        {
            var hataDetaylari = new ValidationProblemDetails(dogrulamaSonucu.ToDictionary())
            {
                Title = "Gönderilen bilgiler geçersiz.",
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext.Request.Path.Value
            };

            return BadRequest(hataDetaylari);
        }

        var sonuc = _rezervasyonService.RezervasyonHesapla(request);

        return Ok(sonuc);
    }
}
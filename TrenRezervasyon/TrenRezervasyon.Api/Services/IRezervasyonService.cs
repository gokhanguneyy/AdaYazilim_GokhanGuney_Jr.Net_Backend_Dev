using TrenRezervasyon.Api.DTOs.Requests;
using TrenRezervasyon.Api.DTOs.Responses;

namespace TrenRezervasyon.Api.Services;

public interface IRezervasyonService
{
    RezervasyonResponseDto RezervasyonHesapla(RezervasyonRequestDto request);
}
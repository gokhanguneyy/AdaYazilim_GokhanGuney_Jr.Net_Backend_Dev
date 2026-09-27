namespace TrenRezervasyon.Api.DTOs.Responses
{
    public class RezervasyonResponseDto
    { 
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimAyrintiDto> YerlesimAyrinti { get; set; } = new List<YerlesimAyrintiDto>();
    }
}

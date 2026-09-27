namespace TrenRezervasyon.Api.DTOs.Requests
{
    public class RezervasyonRequestDto
    {
        public TrenDto Tren { get; set; } = new TrenDto();

        public int RezervasyonYapilacakKisiSayisi { get; set; }

        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}

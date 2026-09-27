namespace TrenRezervasyon.Api.DTOs.Requests
{
    public class TrenDto
    {
        public string Ad { get; set; } = string.Empty;
        public List<VagonDto> Vagonlar { get; set; } = new List<VagonDto>();
    }
}

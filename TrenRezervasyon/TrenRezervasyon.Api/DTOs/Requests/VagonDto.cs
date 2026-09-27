namespace TrenRezervasyon.Api.DTOs.Requests
{
    public class VagonDto
    {
        public string Ad { get; set; } = string.Empty;
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }
    }
}

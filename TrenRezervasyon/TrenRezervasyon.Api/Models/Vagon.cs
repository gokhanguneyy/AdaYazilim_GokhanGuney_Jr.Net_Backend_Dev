namespace TrenRezervasyon.Api.Models
{
    public class Vagon
    {
        public string Ad { get; set; } = string.Empty;
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }
        public int KullanilabilirKoltukSayisiHesapla()
        {
            int onlineKoltukLimiti = (int)Math.Floor((Kapasite * 70m) / 100);
            return Math.Max(0, onlineKoltukLimiti - DoluKoltukAdet);
        }
    }
}

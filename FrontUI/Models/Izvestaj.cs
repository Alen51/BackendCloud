namespace FrontUI.Models
{
    public class Izvestaj
    {
        public List<OcenaNaPredmetu> ocene { get; set; } = new List<OcenaNaPredmetu>();

        public double SrednjaOcena { get; set; } = 0;
    }
}

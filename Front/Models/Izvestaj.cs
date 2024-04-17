namespace Front.Models
{
    public class Izvestaj
    {
        public List<OcenaNaPredmetu> ocene { get; set; }

        public double SrednjaOcena { get; set; } = 0;
    }
}

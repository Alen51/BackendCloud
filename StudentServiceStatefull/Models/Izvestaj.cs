using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceStatefull.Models
{
    public class Izvestaj
    {


        public List<OcenaNaPredmetu> ocene { get; set; } = new List<OcenaNaPredmetu>();

        public double SrednjaOcena { get; set; }
    }
}

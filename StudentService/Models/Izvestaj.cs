using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService.Models
{
    public class Izvestaj
    {
       

        public List<OcenaNaPredmetu> ocene {  get; set; }

        public double SrednjaOcena { get; set; }
    }
}

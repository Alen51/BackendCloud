using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceStatefull.Models
{
    public class PredmetDto
    {
        public Predmet Predmet { get; set; }= new Predmet();
        public bool Upisan { get; set; }=false;
    }
}

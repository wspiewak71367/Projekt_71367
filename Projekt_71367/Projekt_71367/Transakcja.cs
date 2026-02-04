using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_71367
{
    public class Transakcja
    {
        public Produkt Produkt { get; set; }
        public DateTime Data { get; set; }
        public decimal Kwota { get; set; }
    }
}

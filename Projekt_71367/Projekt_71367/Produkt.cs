using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_71367
{
    public class Produkt
    {
        public string Nazwa { get; set; }
        public decimal Cena { get; set; }
        public int Ilosc { get; set; }

        public Produkt(string nazwa, decimal cena, int ilosc)
        {
            Nazwa = nazwa;
            Cena = cena;
            Ilosc = ilosc;
        }

        public virtual void WyswietlInfo()
        {
            Console.WriteLine($"{Nazwa} - {Cena} zł - Ilość: {Ilosc}");
        }
    }

    public class Gazowany : Produkt
    {
        public bool CzySlodzony { get; set; }

        public Gazowany(string nazwa, decimal cena, int ilosc, bool czySlodzony)
            : base(nazwa, cena, ilosc)
        {
            CzySlodzony = czySlodzony;
        }

        public override void WyswietlInfo()
        {
            string cukier = CzySlodzony ? "słodzony" : "bez cukru";
            Console.WriteLine($"{Nazwa} (gazowany, {cukier}) - {Cena} zł - Ilość: {Ilosc}");
        }
    }

    public class NieGazowany : Produkt
    {
        public string Smak { get; set; }

        public NieGazowany(string nazwa, decimal cena, int ilosc, string smak)
            : base(nazwa, cena, ilosc)
        {
            Smak = smak;
        }

        public override void WyswietlInfo()
        {
            Console.WriteLine($"{Nazwa} (niegazowany, smak: {Smak}) - {Cena} zł - Ilość: {Ilosc}");
        }
    }

    public class Sok : Produkt
    {
        public string Owoc { get; set; }

        public Sok(string nazwa, decimal cena, int ilosc, string owoc)
            : base(nazwa, cena, ilosc)
        {
            Owoc = owoc;
        }

        public override void WyswietlInfo()
        {
            Console.WriteLine($"{Nazwa} (sok, owoc: {Owoc}) - {Cena} zł - Ilość: {Ilosc}");
        }
    }

    public class Energetyk : Produkt
    {
        public int KofeinaMg { get; set; }

        public Energetyk(string nazwa, decimal cena, int ilosc, int kofeinaMg)
            : base(nazwa, cena, ilosc)
        {
            KofeinaMg = kofeinaMg;
        }

        public override void WyswietlInfo()
        {
            Console.WriteLine($"{Nazwa} (energetyk, {KofeinaMg}mg kofeiny) - {Cena} zł - Ilość: {Ilosc}");
        }
    }

}

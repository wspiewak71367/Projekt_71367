using Projekt_71367;
using System;
using System.Collections.Generic;

namespace AutomatNapoje
{
    public class Automat
    {
        // Lista produktów w automacie
        public List<Produkt> Produkty { get; set; } = new List<Produkt>();

        // Historia transakcji
        public List<Transakcja> Transakcje { get; set; } = new List<Transakcja>();

        public void DodajProdukt(Produkt produkt)
        {
            if (produkt != null)
            {
                Produkty.Add(produkt);
                Console.WriteLine($"Dodano produkt: {produkt.Nazwa}");
            }
            else
            {
                Console.WriteLine("Błąd: nieprawidłowy produkt.");
            }
        }
        public void UsunProdukt(int indeks)
        {
            if (indeks >= 0 && indeks < Produkty.Count)
            {
                Console.WriteLine($"Usunięto produkt: {Produkty[indeks].Nazwa}");
                Produkty.RemoveAt(indeks);
            }
            else
            {
                Console.WriteLine("Błąd: nieprawidłowy numer produktu.");
            }
        }
        public void WyswietlProdukty()
        {
            if (Produkty.Count == 0)
            {
                Console.WriteLine("Brak produktów w automacie.");
                return;
            }

            Console.WriteLine("=== DOSTĘPNE PRODUKTY ===");
            for (int i = 0; i < Produkty.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Produkty[i].WyswietlInfo();
            }
        }
        public void KupProdukt(int indeks, decimal kwota)
        {
            if (indeks < 0 || indeks >= Produkty.Count)
            {
                Console.WriteLine("Nieprawidłowy wybór produktu.");
                return;
            }

            Produkt produkt = Produkty[indeks];

            if (produkt.Ilosc <= 0)
            {
                Console.WriteLine("Produkt niedostępny.");
                return;
            }

            if (kwota < produkt.Cena)
            {
                Console.WriteLine($"Za mała kwota. Produkt kosztuje: {produkt.Cena} zł");
                return;
            }

            // Wydaj produkt
            produkt.Ilosc--;

            // Zapisz transakcję
            Transakcje.Add(new Transakcja
            {
                Produkt = produkt,
                Kwota = produkt.Cena,
                Data = DateTime.Now
            });

            Console.WriteLine($"Wydano produkt: {produkt.Nazwa}. Reszta: {kwota - produkt.Cena} zł");
        }
        public void WyswietlTransakcje()
        {
            if (Transakcje.Count == 0)
            {
                Console.WriteLine("Brak transakcji.");
                return;
            }

            Console.WriteLine("=== HISTORIA TRANSAKCJI ===");
            foreach (var t in Transakcje)
            {
                Console.WriteLine($"{t.Data}: {t.Produkt.Nazwa} - {t.Kwota} zł");
            }
        }
    }
}

using Projekt_71367;
using System;
using System.Collections.Generic;

namespace AutomatNapoje
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Automat automat = new Automat();
            PlikManager plikManager = new PlikManager();

            // Wczytaj produkty i transakcje z plików
            automat.Produkty = plikManager.OdczytajProdukty();
            automat.Transakcje = plikManager.OdczytajTransakcje();

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("=== AUTOMAT Z NAPOJAMI ===");
                Console.WriteLine("1. Kup napój");
                Console.WriteLine("2. Menu administratora");
                Console.WriteLine("3. Wyjście");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        KupNapoj(automat);
                        break;

                    case "2":
                        MenuAdministratora(automat, plikManager);
                        break;

                    case "3":
                        exit = true;
                        // Zapisz dane przed wyjściem
                        plikManager.ZapiszProdukty(automat.Produkty);
                        plikManager.ZapiszTransakcje(automat.Transakcje);
                        Console.WriteLine("Do widzenia!");
                        break;

                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }

        // MENU UŻYTKOWNIKA - kupowanie napojów
        static void KupNapoj(Automat automat)
        {
            if (automat.Produkty.Count == 0)
            {
                Console.WriteLine("Brak produktów w automacie.");
                return;
            }

          
            automat.WyswietlProdukty();
            Console.Write("Wybierz numer produktu: ");
            if (!int.TryParse(Console.ReadLine(), out int indeks))
            {
                Console.WriteLine("Nieprawidłowa wartość.");
                return;
            }

            indeks -= 1;  // Konwertuje numer wybrany przez użytkownika na indeks listy w C#, czyli od 0

            if (indeks < 0 || indeks >= automat.Produkty.Count)
            {
                Console.WriteLine("Nieprawidłowy numer produktu.");
                return;
            }

            Produkt produkt = automat.Produkty[indeks];

            if (produkt.Ilosc <= 0)
            {
                Console.WriteLine("Produkt niedostępny.");
                return; 
            }

            Console.Write("Wrzuć monetę (kwota w zł): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal kwota))
            {
                Console.WriteLine("Nieprawidłowa wartość.");
                return;
            }

            automat.KupProdukt(indeks, kwota);
        }

        // MENU ADMINISTRATORA
        static void MenuAdministratora(Automat automat, PlikManager plikManager)
        {
            bool back = false;

            while (!back)
            {
                Console.WriteLine("=== MENU ADMINISTRATORA ===");
                Console.WriteLine("1. Dodaj produkt");
                Console.WriteLine("2. Usuń produkt");
                Console.WriteLine("3. Wyświetl produkty");
                Console.WriteLine("4. Wyświetl transakcje");
                Console.WriteLine("5. Edytuj produkt (cena / ilość)");
                Console.WriteLine("6. Powrót");
                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DodajProdukt(automat);
                        break;

                    case "2":
                        UsunProdukt(automat);
                        break;

                    case "3":
                        automat.WyswietlProdukty();
                        break;

                    case "4":
                        automat.WyswietlTransakcje();
                        break;

                    case "5":
                        EdytujProdukt(automat);
                        break;

                    case "6":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }


            // Zapisz produkty i transakcje po zmianach administratora
            plikManager.ZapiszProdukty(automat.Produkty);
            plikManager.ZapiszTransakcje(automat.Transakcje);
        }

        // DODAWANIE PRODUKTU
        static void DodajProdukt(Automat automat)
        {
            Console.WriteLine("Typy produktów: 1. Gazowany  2. NieGazowany  3. Sok  4. Energetyk");
            Console.Write("Wybierz typ produktu: ");
            string typ = Console.ReadLine();

            Console.Write("Nazwa: ");
            string nazwa = Console.ReadLine();

            Console.Write("Cena: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cena))
            {
                Console.WriteLine("Nieprawidłowa cena.");
                return;
            }

            Console.Write("Ilość: ");
            if (!int.TryParse(Console.ReadLine(), out int ilosc))
            {
                Console.WriteLine("Nieprawidłowa ilość.");
                return;
            }

            Produkt produkt = typ switch
            {
                "1" => new Gazowany(nazwa, cena, ilosc, CzySlodzony()),
                "2" => new NieGazowany(nazwa, cena, ilosc, Smak()),
                "3" => new Sok(nazwa, cena, ilosc, Owoc()),
                "4" => new Energetyk(nazwa, cena, ilosc, Kofeina()),
                _ => null
            };

            if (produkt != null)
            {
                automat.DodajProdukt(produkt);
            }
            else
            {
                Console.WriteLine("Nieprawidłowy typ produktu.");
            }
        }

        static bool CzySlodzony()
        {
            Console.Write("Czy słodzony? (t/n): ");
            string s = Console.ReadLine().ToLower();
            return s == "t";
        }

        static string Smak()
        {
            Console.Write("Podaj smak: ");
            return Console.ReadLine();
        }

        static string Owoc()
        {
            Console.Write("Podaj owoc: ");
            return Console.ReadLine();
        }

        static int Kofeina()
        {
            Console.Write("Podaj ilość kofeiny (mg): ");
            if (int.TryParse(Console.ReadLine(), out int mg))
                return mg;
            return 0;
        }

        // USUWANIE PRODUKTU
        static void UsunProdukt(Automat automat)
        {
            automat.WyswietlProdukty();
            Console.Write("Podaj numer produktu do usunięcia: ");
            if (!int.TryParse(Console.ReadLine(), out int indeks))
            {
                Console.WriteLine("Nieprawidłowa wartość.");
                return;
            }

            indeks -= 1; // Konwertuje numer wybrany przez użytkownika na indeks listy w C#, czyli od 0
            automat.UsunProdukt(indeks);
        }

        static void EdytujProdukt(Automat automat)
        {
            if (automat.Produkty.Count == 0)
            {
                Console.WriteLine("Brak produktów do edycji.");
                return;
            }

            automat.WyswietlProdukty();
            Console.Write("Podaj numer produktu do edycji: ");
            if (!int.TryParse(Console.ReadLine(), out int indeks))
            {
                Console.WriteLine("Nieprawidłowa wartość.");
                return;
            }

            indeks--; // Konwertuje numer wybrany przez użytkownika na indeks listy w C#, czyli od 0

            if (indeks < 0 || indeks >= automat.Produkty.Count)
            {
                Console.WriteLine("Nieprawidłowy numer produktu.");
                return;
            }

            Produkt produkt = automat.Produkty[indeks];

            Console.Write($"Nowa cena produktu {produkt.Nazwa} (aktualna {produkt.Cena} zł): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal nowaCena))
                produkt.Cena = nowaCena;

            Console.Write($"Nowa ilość produktu {produkt.Nazwa} (aktualna {produkt.Ilosc}): ");
            if (int.TryParse(Console.ReadLine(), out int nowaIlosc))
                produkt.Ilosc = nowaIlosc;

            Console.WriteLine($"Produkt {produkt.Nazwa} został zaktualizowany. Cena: {produkt.Cena} zł, Ilość: {produkt.Ilosc}");
        }

    }
}

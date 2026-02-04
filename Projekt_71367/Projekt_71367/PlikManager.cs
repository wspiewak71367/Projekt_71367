using Projekt_71367;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomatNapoje
{
    public class PlikManager
    {
        private const string ProduktyPlik = "produkty.json";
        private const string TransakcjePlik = "transakcje.json";

        // Zapis produktów do pliku
        public void ZapiszProdukty(List<Produkt> produkty)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true,
                    Converters = { new ProduktConverter() }
                };

                string json = JsonSerializer.Serialize(produkty, options);
                File.WriteAllText(ProduktyPlik, json);
                Console.WriteLine("Produkty zapisane.");
            }
            catch
            {
                Console.WriteLine("Nie udało się zapisać produktów.");
            }
        }

        // Wczytanie produktów z pliku
        public List<Produkt> OdczytajProdukty()
        {
            try
            {
                if (!File.Exists(ProduktyPlik))
                    return new List<Produkt>();

                string json = File.ReadAllText(ProduktyPlik);
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    Converters = { new ProduktConverter() }
                };

                return JsonSerializer.Deserialize<List<Produkt>>(json, options) ?? new List<Produkt>();
            }
            catch
            {
                Console.WriteLine("Nie udało się wczytać produktów.");
                return new List<Produkt>();
            }
        }

        // Zapis transakcji do pliku
        public void ZapiszTransakcje(List<Transakcja> transakcje)
        {
            try
            {
                string json = JsonSerializer.Serialize(transakcje, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(TransakcjePlik, json);
                Console.WriteLine("Transakcje zapisane.");
            }
            catch
            {
                Console.WriteLine("Nie udało się zapisać transakcji.");
            }
        }

        // Wczytanie transakcji z pliku
        public List<Transakcja> OdczytajTransakcje()
        {
            try
            {
                if (!File.Exists(TransakcjePlik))
                    return new List<Transakcja>();

                string json = File.ReadAllText(TransakcjePlik);
                return JsonSerializer.Deserialize<List<Transakcja>>(json) ?? new List<Transakcja>();
            }
            catch
            {
                Console.WriteLine("Nie udało się wczytać transakcji.");
                return new List<Transakcja>();
            }
        }

        // Konwerter produktów dla JSON (dziedziczenie)
        public class ProduktConverter : JsonConverter<Produkt>
        {
            public override Produkt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var root = JsonDocument.ParseValue(ref reader).RootElement;

                string typ = root.GetProperty("Typ").GetString();
                string nazwa = root.GetProperty("Nazwa").GetString();
                decimal cena = root.GetProperty("Cena").GetDecimal();
                int ilosc = root.GetProperty("Ilosc").GetInt32();

                return typ switch
                {
                    "Gazowany" => new Gazowany(nazwa, cena, ilosc, root.GetProperty("CzySlodzony").GetBoolean()),
                    "NieGazowany" => new NieGazowany(nazwa, cena, ilosc, root.GetProperty("Smak").GetString()),
                    "Sok" => new Sok(nazwa, cena, ilosc, root.GetProperty("Owoc").GetString()),
                    "Energetyk" => new Energetyk(nazwa, cena, ilosc, root.GetProperty("KofeinaMg").GetInt32()),
                    _ => new Produkt(nazwa, cena, ilosc)
                };
            }

            public override void Write(Utf8JsonWriter writer, Produkt value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                if (value is Gazowany g)
                {
                    writer.WriteString("Typ", "Gazowany");
                    writer.WriteString("Nazwa", g.Nazwa);
                    writer.WriteNumber("Cena", g.Cena);
                    writer.WriteNumber("Ilosc", g.Ilosc);
                    writer.WriteBoolean("CzySlodzony", g.CzySlodzony);
                }
                else if (value is NieGazowany ng)
                {
                    writer.WriteString("Typ", "NieGazowany");
                    writer.WriteString("Nazwa", ng.Nazwa);
                    writer.WriteNumber("Cena", ng.Cena);
                    writer.WriteNumber("Ilosc", ng.Ilosc);
                    writer.WriteString("Smak", ng.Smak);
                }
                else if (value is Sok s)
                {
                    writer.WriteString("Typ", "Sok");
                    writer.WriteString("Nazwa", s.Nazwa);
                    writer.WriteNumber("Cena", s.Cena);
                    writer.WriteNumber("Ilosc", s.Ilosc);
                    writer.WriteString("Owoc", s.Owoc);
                }
                else if (value is Energetyk e)
                {
                    writer.WriteString("Typ", "Energetyk");
                    writer.WriteString("Nazwa", e.Nazwa);
                    writer.WriteNumber("Cena", e.Cena);
                    writer.WriteNumber("Ilosc", e.Ilosc);
                    writer.WriteNumber("KofeinaMg", e.KofeinaMg);
                }
                else
                {
                    writer.WriteString("Typ", "Produkt");
                    writer.WriteString("Nazwa", value.Nazwa);
                    writer.WriteNumber("Cena", value.Cena);
                    writer.WriteNumber("Ilosc", value.Ilosc);
                }

                writer.WriteEndObject();
            }
        }
    }
}

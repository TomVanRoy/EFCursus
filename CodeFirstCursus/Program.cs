using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstCursus
{
    internal class Program
    {
        private static void CampusInstructeur()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VDABContext>());
            using (var context = new VDABContext())
            {
                var campus = new Campus
                {
                    Adres = new Adres { Gemeente = "Wevelgem", HuisNr = "10", PostCode = "8560", Straat = "Vlamingstraat" },
                    Naam = "Delos"
                };
                var jean = new Instructeur
                {
                    Adres = new Adres { Gemeente = "Brussel", HuisNr = "11", PostCode = "1000", Straat = "Keizerslaan" },
                    Campus = campus,
                    Familienaam = "Smits",
                    HeeftRijbewijs = true,
                    InDienst = new DateTime(1966, 8, 1),
                    Voornaam = "Jean",
                    Wedde = 1000
                };
                var verantwoordelijkheid = new Verantwoordelijkheid { Naam = "EHBO" };
                jean.Verantwoordelijkheden = new List<Verantwoordelijkheid> { verantwoordelijkheid };
                context.Campussen.Add(campus);
                context.Instructeurs.Add(jean);
                context.Verantoordelijkheden.Add(verantwoordelijkheid);
                context.SaveChanges();
            }
        }

        private static void Cursisten()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VDABContext>());
            using (var context = new VDABContext())
            {
                Cursist joe = new Cursist { Voornaam = "Joe", Familienaam = "Dalton" };
                Cursist averell = new Cursist { Voornaam = "Averell", Familienaam = "Dalton", Mentor = joe };
                context.Cursisten.Add(joe);
                context.Cursisten.Add(averell);
                context.SaveChanges();
            }
        }

        private static void CursusToevoegen()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VDABContext>());
            using (var context = new VDABContext())
            {
                context.Cursussen.Add(new KlassikaleCursus { Naam = "Frans in 24 uur", Van = DateTime.Today, Tot = DateTime.Today });
                context.Cursussen.Add(new ZelfstudieCursus { Naam = "Engels in 24 uur", AantalDagen = 1 });
                context.SaveChanges();
            }
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Druk op enter om het programma af te sluiten . . .");
            Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            Cursisten();
            Exit();
        }

        private void AddJean()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VDABContext>());
            using (var context = new VDABContext())
            {
                var jean = new Instructeur
                {
                    Voornaam = "Jean",
                    Familienaam = "Smits",
                    Wedde = 1000,
                    InDienst = new DateTime(1994, 8, 1),
                    HeeftRijbewijs = true,
                    Adres = new Adres { Straat = "Keizerslaan", HuisNr = "11", PostCode = "1000", Gemeente = "Brussel" }
                };
                context.Instructeurs.Add(jean);
                context.SaveChanges();
                Console.WriteLine(jean.InstructeurNr);
                Console.WriteLine(context.Instructeurs.Find(1).Familienaam);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;

namespace EFCursus
{
    internal class Program
    {
        private static void AssociatieWijzigenVanuitEenKant()
        {
            using (var entities = new OpleidingenEntities())
            {
                var docent1 = entities.Docenten.Find(1);
                if (docent1 != null)
                {
                    var campus3 = entities.Campussen.Find(3);
                    if (campus3 != null)
                    {
                        campus3.Docenten.Add(docent1);
                        entities.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Campus niet gevonden");
                    }
                }
                else
                {
                    Console.WriteLine("Docent niet gevonden");
                }
            }
        }

        private static void AssociatieWijzigenVanuitVeelKant()
        {
            using (var entities = new OpleidingenEntities())
            {
                var docent1 = entities.Docenten.Find(1);
                if (docent1 != null)
                {
                    /*
                var campus6 = entities.Campussen.Find(6);
                if (campus6 != null)
                {
                    docent1.Campus = campus6;
                    */
                    docent1.CampusNr = 2;
                    entities.SaveChanges();
                    /*
                }
                else
                {
                    Console.WriteLine("Campus niet gevonden");
                }
                */
                }
                else
                {
                    Console.WriteLine("Docent niet gevonden");
                }
            }
        }

        private static void CampussenVanTotPostCode()
        {
            using (var entities = new OpleidingenEntities())
            {
                foreach (var campus in entities.CampussenVanTotPostCode("8000", "8999"))
                {
                    Console.WriteLine("{0}: {1}", campus.Naam, campus.PostCode);
                }
            }
        }

        private static void CampussenVanTotPostCode2()
        {
            using (var entities = new OpleidingenEntities())
            {
                foreach (var voornaamAantal in entities.AantalDocentenPerVoornaam())
                {
                    Console.WriteLine("{0} {1}", voornaamAantal.Voornaam, voornaamAantal.Aantal);
                }
            }
        }

        private static void CampussenVanTotPostCode3()
        {
            Console.Write("Opslagpercentage: ");
            decimal percentage;
            if (decimal.TryParse(Console.ReadLine(), out percentage))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var aantalDocentenAangepast = entities.WeddeVerhoging(percentage);
                    Console.WriteLine("{0} docenten aangepast", aantalDocentenAangepast);
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
        }

        private static void CampussenVanTotPostCode4()
        {
            Console.Write("Famalienaam: ");
            var familienaam = Console.ReadLine();
            using (var entities = new OpleidingenEntities())
            {
                var aantalDocenten = entities.AantalDocentenMetFamilienaam(familienaam);
                Console.WriteLine("{0} docent(en)", aantalDocenten.First());
            }
        }

        private static void ComplexType()
        {
            using (var entities = new OpleidingenEntities())
            {
                foreach (var cursist in (from eenCursist in entities.Cursisten select eenCursist))
                {
                    Console.WriteLine(cursist.Naam.InformeleBegroeting);
                }
            }
        }

        private static void EagerLoading()
        {
            using (var entities = new OpleidingenEntities())
            {
                Console.Write("Wil je zoeken via naam docent [1] of naam campus [2]? ");
                string response = Console.ReadLine();

                switch (response)
                {
                    case "1":
                        Console.Write("Voornaam: ");
                        var voornaam = Console.ReadLine();
                        var query1 = from docent in entities.Docenten.Include("Campus")
                                     where docent.Naam.Voornaam == voornaam
                                     select docent;

                        foreach (var docent in query1)
                        {
                            Console.WriteLine("{0}: {1}", docent.Naam, docent.Campus.Naam);
                        }
                        break;

                    case "2":
                        Console.Write("Deel naam campus: ");
                        var deelNaam = Console.ReadLine();
                        /*
                        var query2 = from campus in entities.Campussen.Include("Docenten")
                                     where campus.Naam.Contains(deelNaam)
                                     orderby campus.Naam
                                     select campus;
                        */
                        var query2 = entities.Campussen.Include("Docenten")
                                .Where(campus => campus.Naam.Contains(deelNaam))
                                .OrderBy(campus => campus.Naam);

                        foreach (var campus in query2)
                        {
                            var campusNaam = campus.Naam;
                            Console.WriteLine(new string('-', campusNaam.Length));
                            Console.WriteLine(campusNaam);
                            Console.WriteLine(new string('-', campusNaam.Length));
                            foreach (var docent in campus.Docenten)
                            {
                                Console.WriteLine(docent.Naam);
                            }
                            Console.WriteLine();
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private static void EenEntityToevoegen()
        {
            var campus = new Campus
            {
                Naam = "Naam1",
                Straat = "Straat1",
                HuisNr = "1",
                PostCode = "1111",
                Gemeente = "Gemeente1"
            };
            using (var entities = new OpleidingenEntities())
            {
                entities.Campussen.Add(campus);
                entities.SaveChanges();
                Console.WriteLine(campus.CampusNr);
            }
        }

        private static void EenEntityWijzigen()
        {
            Console.WriteLine("DocumentNr.: ");
            int docentNr;
            if (int.TryParse(Console.ReadLine(), out docentNr))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var docent = entities.Docenten.Find(docentNr);
                    if (docent != null)
                    {
                        Console.WriteLine("Wedde: {0}", docent.Wedde);
                        Console.Write("Bedrag: ");
                        decimal bedrag;
                        if (decimal.TryParse(Console.ReadLine(), out bedrag))
                        {
                            docent.Opslag(bedrag);
                            entities.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Tik een getal");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Docent niet gevonden");
                    }
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
        }

        private static void EntitiesGebruikenVanuitCode()
        {
            /*
            // voorbeeld 1
            using (var entities = new OpleidingenEntities())
            {
                var query = from boek in entities.Boeken.Include("Cursussen")
                            orderby boek.Titel
                            select boek;
                foreach (var boek in query)
                {
                    Console.WriteLine(boek.Titel);
                    foreach (var cursus in boek.Cursussen)
                    {
                        Console.WriteLine("\t{0}", cursus.Naam);
                    }
                }
            }

            // voorbeeld 2
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursus in entities.Cursussen.Include("Boeken")
                            orderby cursus.CursusNr
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                    foreach (var boek in cursus.Boeken)
                    {
                        Console.WriteLine("\t{0}", boek.Titel);
                    }
                }
            }

            // voorbeeld 3
            using (var entities = new OpleidingenEntities())
            {
                var nieuwBoek = new Boek
                {
                    ISBNNr = "0-0788210-6-1",
                    Titel = "Oracle Backup & Recovery Handbook"
                };
                var oracleCursus = (from cursus in entities.Cursussen
                                    where cursus.Naam == "Oracle"
                                    select cursus).FirstOrDefault();
                if (oracleCursus != null)
                {
                    oracleCursus.Boeken.Add(nieuwBoek);
                    entities.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Cursus niet gevonden");
                }
            }

            // voorbeeld 4
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursus in entities.Cursussen.Include("BoekenCursussen.Boek")
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                    foreach (var boekCursus in cursus.BoekenCursussen)
                    {
                        Console.WriteLine("\t{0}:{1}", boekCursus.VolgNr, boekCursus.Boek.Titel);
                    }
                }
            }
            // voorbeeld 5
            var nieuwBoek = new Boek() { ISBNNr = "0-201-70431-5", Titel = "Modern C++ Design" };
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var query = from cursus in entities.Cursussen.Include("BoekenCursussen")
                                where cursus.Naam == "C++"
                                select new { Cursus = cursus, HoogsteVolgNr = cursus.BoekenCursussen.Max(boekCursus => boekCursus.VolgNr) };
                    var queryResult = query.FirstOrDefault();
                    if (queryResult != null)
                    {
                        entities.BoekenCursussen.Add(new BoekCursus
                        {
                            Boek = nieuwBoek,
                            Cursus = queryResult.Cursus,
                            VolgNr = queryResult.HoogsteVolgNr + 1
                        });
                        entities.SaveChanges();
                    }
                    transactionScope.Complete();
                }
            }
            */
            /*
            // voorbeeld 6
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursist in entities.Cursisten
                            where cursist.Mentor == null
                            orderby cursist.Voornaam, cursist.Familienaam
                            select cursist;
                foreach (var cursist in query)
                {
                    Console.WriteLine("{0} {1}", cursist.Voornaam, cursist.Familienaam);
                }
            }
            */
            /*
            // voorbeeld 7
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursist in entities.Cursisten.Include("Mentor")
                            where cursist.Mentor != null
                            orderby cursist.Voornaam, cursist.Familienaam
                            select cursist;
                foreach (var cursist in query)
                {
                    var mentor = cursist.Mentor;
                    Console.WriteLine("{0} {1}: {2} {3}", cursist.Voornaam, cursist.Familienaam, mentor.Voornaam, mentor.Familienaam);
                }
            }
            */
            /*
            // voorbeeld 8
            using (var entities = new OpleidingenEntities())
            {
                var query = from mentor in entities.Cursisten.Include("Beschermelingen")
                            where mentor.Beschermelingen.Count != 0
                            orderby mentor.Voornaam, mentor.Familienaam
                            select mentor;
                foreach (var mentor in query)
                {
                    Console.WriteLine("{0} {1}", mentor.Voornaam, mentor.Familienaam);
                    foreach (var beschermeling in mentor.Beschermelingen)
                    {
                        Console.WriteLine("\t{0} {1}", beschermeling.Voornaam, beschermeling.Familienaam);
                    }
                }
            }
            */
            /*
            // voorbeeld 9
            using (var entities = new OpleidingenEntities())
            {
                var cursist5 = entities.Cursisten.Find(5);
                if (cursist5 != null)
                {
                    var cursist6 = entities.Cursisten.Find(6);
                    if (cursist6 != null)
                    {
                        cursist5.Beschermelingen.Add(cursist6);
                        entities.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Cursist 6 niet gevonden");
                    }
                }
                else
                {
                    Console.WriteLine("Cursist 5 niet gevonden");
                }
            }
            */
            /*
            // voorbeeld 10
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursus in entities.Cursussen
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                }
            }
            */
            /*
            // voorbeeld 11
            using (var entities = new OpleidingenEntities())
            {
                var query = from cursus in entities.Cursussen
                            where cursus is KlassikaleCursus
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                }
            }
            */
            /*
            // voorbeeld 12
            using (var entities = new OpleidingenEntities())
            {
                entities.Cursussen.Add(new ZelfstudieCursus { Naam = "Spaanse correspondentie", Duurtijd = 6 });
                entities.SaveChanges();
            }
            */
        }

        private static void EntitiesWijzigenDieIndirectGelezenZijnMetAssociaties()
        {
            using (var entities = new OpleidingenEntities())
            {
                var campus1 = entities.Campussen.Find(1);
                if (campus1 != null)
                {
                    foreach (var docent in campus1.Docenten)
                    {
                        docent.Opslag(10m);
                    }
                    entities.SaveChanges();
                }
            }
        }

        private static void EntitiesWijzigigen()
        {
            Console.Write("Nummer docent: ");
            int docentnr;
            if (int.TryParse(Console.ReadLine(), out docentnr))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var docent = entities.Docenten.Find(docentnr);
                    if (docent != null)
                    {
                        entities.Docenten.Remove(docent);
                        entities.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Docent niet gevonden");
                    }
                }
            }
            else
            {
                Console.WriteLine("Tik een geheel getal");
            }
        }

        /*
        private static void EntityToevoegenEnAssociatieDefinierenVanuitVeelKant()
        {
            var docent3 = new Docent
            {
                Voornaam = "Voornaam3",
                Familienaam = "Familienaam3",
                Wedde = 3
            };
            var docent4 = new Docent
            {
                Voornaam = "Voornaam4",
                Familienaam = "Familienaam4",
                Wedde = 4,
                CampusNr = 1
            };
            var docent5 = new Docent
            {
                Voornaam = "Voornaam5",
                Familienaam = "Familienaam5",
                Wedde = 5
            };
            using (var entities = new OpleidingenEntities())
            {
                var campus1 = entities.Campussen.Find(1);
                if (campus1 != null)
                {
                    entities.Docenten.Add(docent3);
                    entities.Docenten.Add(docent4);
                    campus1.Docenten.Add(docent5);
                    docent3.Campus = campus1;
                    entities.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Campus 1 niet gevonden");
                }
            }
        }
        */

        private static void Enumeration()
        {
            using (var entities = new OpleidingenEntities())
            {
                /*
                foreach (var docent in entities.Docenten)
                {
                    Console.WriteLine("{0}: {1}", docent.Naam, docent.Geslacht);
                }
                */
                entities.Docenten.Add(new Docent { Naam = new Naam { Voornaam = "Brigitta", Familienaam = "Roos" }, Wedde = 2000, Geslacht = Geslacht.Vrouw, CampusNr = 1 });
                entities.SaveChanges();
            }
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Druk op een toets om het programma af te sluiten . . . ");
            Console.Read();
        }

        private static void GroupedQuery()
        {
            using (var entities = new OpleidingenEntities())
            {
                var query = from docent in entities.Docenten
                            group docent by docent.Naam.Voornaam into VoornaamGroep
                            select new { VoornaamGroep, Voornaam = VoornaamGroep.Key };
                /*
                var query = entities.Docenten
                            .GroupBy((docent) => docent.Voornaam,
                            (Voornaam, docenten) => new { Voornaam, VoornaamGroup = docenten });
                */
                foreach (var voornaamStatistiek in query)
                {
                    if (voornaamStatistiek.VoornaamGroep.Count() > 1)
                    {
                        Console.WriteLine(voornaamStatistiek.Voornaam);
                        Console.WriteLine(new string('-', voornaamStatistiek.Voornaam.Length));
                    }
                    foreach (var docent in voornaamStatistiek.VoornaamGroep)
                    {
                        Console.WriteLine(docent.Naam);
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void IngebouwdeTransactiebeheer()
        {
            try
            {
                Console.WriteLine("Artikel nr.: ");
                var artikelNr = int.Parse(Console.ReadLine());
                Console.WriteLine("Van magazijn nr.: ");
                var vanMagazijnNr = int.Parse(Console.ReadLine());
                Console.WriteLine("Naar magazijn nr.: ");
                var naarMagazijnNr = int.Parse(Console.ReadLine());
                Console.WriteLine("Aantal stuks: ");
                var aantalStuks = int.Parse(Console.ReadLine());
                VoorraadTransfer(artikelNr, vanMagazijnNr, naarMagazijnNr, aantalStuks);
            }
            catch (FormatException)
            {
                Console.WriteLine("Tik een getal");
            }
        }

        private static void LazyLoading()
        {
            using (var entities = new OpleidingenEntities())
            {
                Console.Write("Voornaam: ");
                var voornaam = Console.ReadLine();
                var query = from docent in entities.Docenten
                            where docent.Naam.Voornaam == voornaam
                            select docent;

                foreach (var docent in query)
                {
                    Console.WriteLine("{0}: {1}", docent.Naam, docent.Campus.Naam);
                }
            }
        }

        private static void LINQ()
        {
            Console.Write("Minimum wedde: ");
            decimal minWedde;
            if (decimal.TryParse(Console.ReadLine(), out minWedde))
            {
                Console.Write("Sorteren: 1=op wedde, 2=op familienaam, 3=op voornaam: ");
                var sorterenOp = Console.ReadLine();
                Func<Docent, object> sorteerLambda;

                switch (sorterenOp)
                {
                    case "1":
                        sorteerLambda = (docent) => docent.Wedde;
                        break;

                    case "2":
                        sorteerLambda = (docent) => docent.Naam.Familienaam;
                        break;

                    case "3":
                        sorteerLambda = (docent) => docent.Naam.Voornaam;
                        break;

                    default:
                        Console.WriteLine("Verkeerde keuze");
                        sorteerLambda = null;
                        break;
                }
                if (sorteerLambda != null)
                {
                    using (var entities = new OpleidingenEntities())
                    {
                        var query = entities.Docenten
                            .Where(docent => docent.Wedde >= minWedde)
                            .OrderBy(sorteerLambda);
                        foreach (var docent in query)
                        {
                            Console.WriteLine("{0}: {1}", docent.Naam, docent.Wedde);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
        }

        private static void Main(string[] args)
        {
            CampussenVanTotPostCode4();
            Exit();
        }

        private static void MeerdereEntitiesLezenEnSlechtsEnkeleDaarvanWijzigen()
        {
            using (var entities = new OpleidingenEntities())
            {
                var docent1 = entities.Docenten.Find(1);
                var docent2 = entities.Docenten.Find(2);
                docent2.Opslag(10m);
                entities.SaveChanges();
            };
        }

        private static void MeerdereEntitiesToevoegen()
        {
            var campus2 = new Campus
            {
                Naam = "Naam2",
                Straat = "Straat2",
                HuisNr = "2",
                PostCode = "2222",
                Gemeente = "Gemeente2"
            };
            var campus3 = new Campus
            {
                Naam = "Naam3",
                Straat = "Straat3",
                HuisNr = "3",
                PostCode = "3333",
                Gemeente = "Gemeente3"
            };
            using (var entities = new OpleidingenEntities())
            {
                entities.Campussen.Add(campus2);
                entities.Campussen.Add(campus3);
                entities.SaveChanges();
            }
        }

        private static void PartialObject()
        {
            using (var entities = new OpleidingenEntities())
            {
                /*
                var query = from campus in entities.Campussen
                            orderby campus.Naam
                            select new { campus.CampusNr, campus.Naam };
                */
                var query = entities.Campussen
                            .OrderBy(campus => campus.Naam)
                            .Select(campus => new { campus.CampusNr, campus.Naam });
                foreach (var campusDeel in query)
                {
                    Console.WriteLine("{0}: {1}", campusDeel.CampusNr, campusDeel.Naam);
                }
            }
        }

        private static void PrimaryKeyFind()
        {
            using (var entities = new OpleidingenEntities())
            {
                Console.Write("Docentnummer: ");
                int docentNr;
                if (int.TryParse(Console.ReadLine(), out docentNr))
                {
                    var docent = entities.Docenten.Find(docentNr);
                    Console.WriteLine(docent == null ? "Niet gevonden" : docent.Naam.ToString());
                }
                else
                {
                    Console.WriteLine("Gelieve een getal in te geven");
                }
            }
        }

        private static void QueryToList()
        {
            List<Campus> campussen;
            using (var entities = new OpleidingenEntities())
            {
                var query = from campus in entities.Campussen
                            orderby campus.Naam
                            select campus;
                campussen = query.ToList();
            }
            foreach (var campus in campussen)
            {
                Console.WriteLine(campus.Naam);
            }
            Console.WriteLine();
            foreach (var campus in campussen)
            {
                Console.WriteLine(campus.Naam);
            }
            Console.WriteLine();
        }

        private static void TablePerHierarchy()
        {
            using (var entities = new OpleidingenEntities())
            {
                /*
                var query = from cursus in entities.Cursussen
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine("{0}: {1}", cursus.Naam, cursus.GetType().Name);
                }
                */
                /*
                var query = from cursus in entities.Cursussen
                            where cursus is ZelfstudieCursus
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                }
                */
                entities.Cursussen.Add(
                    new ZelfstudieCursus { Naam = "Duitse correspondentie", Duurtijd = 6 }
                    );
                entities.SaveChanges();
            }
        }

        private static void TablePerType()
        {
            using (var entities = new OpleidingenEntities())
            {
                /*
                var query = from cursus in entities.Cursussen
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine("{0}: {1}", cursus.Naam, cursus.GetType().Name);
                }
                */
                /*
                var query = from cursus in entities.Cursussen
                            where !(cursus is ZelfstudieCursus)
                            orderby cursus.Naam
                            select cursus;
                foreach (var cursus in query)
                {
                    Console.WriteLine(cursus.Naam);
                }
                */
                entities.Cursussen.Add(
                    new ZelfstudieCursus { Naam = "Italiaanse correspondentie", Duurtijd = 6 }
                    );
                entities.SaveChanges();
            }
        }

        private static void Views()
        {
            using (var entities = new OpleidingenEntities())
            {
                var query = from bestBetaaldeDocentPerCampus
                            in entities.BestBetaaldeDocentenPerCampus
                            orderby bestBetaaldeDocentPerCampus.CampusNr, bestBetaaldeDocentPerCampus.Voornaam, bestBetaaldeDocentPerCampus.Familienaam
                            select bestBetaaldeDocentPerCampus;
                var vorigCampusNr = 0;
                foreach (var bestbetaaldeDocentPerCampus in query)
                {
                    if (bestbetaaldeDocentPerCampus.CampusNr != vorigCampusNr)
                    {
                        Console.WriteLine("{0} {1} Grootste wedde: ", bestbetaaldeDocentPerCampus.Naam, bestbetaaldeDocentPerCampus.GrootsteWedde);
                        vorigCampusNr = bestbetaaldeDocentPerCampus.CampusNr;
                    }
                    Console.WriteLine("\t{0} {1}", bestbetaaldeDocentPerCampus.Voornaam, bestbetaaldeDocentPerCampus.Familienaam);
                }
            }
        }

        /*
        private static void NieuweGeassocieerdeEntities()
        {
            var campus4 = new Campus { Naam = "Naam4", Straat = "Straat4", HuisNr = "4", PostCode = "4444", Gemeente = "Gemeente4" };
            var docent1 = new Docent { Voornaam = "Voornaam1", Familienaam = "Familienaam1", Wedde = 1 };
            campus4.Docenten.Add(docent1);

            var campus5 = new Campus { Naam = "Naam5", Straat = "Straat5", HuisNr = "5", PostCode = "5555", Gemeente = "Gemeente5" };
            var docent2 = new Docent { Voornaam = "Voornaam2", Familienaam = "Familienaam2", Wedde = 2 };
            docent2.Campus = campus5;

            using (var entities = new OpleidingenEntities())
            {
                entities.Campussen.Add(campus4);
                entities.Docenten.Add(docent2);
                entities.SaveChanges();
            }
        }
        */
        /*
        private static List<Campussen> FindAllCampussen()
        {
            using (var entities = new OpleidingenEntities())
            {
                return (from campus in entities.Campussen
                        orderby campus.Naam
                        select campus).ToList();
            }
        }
        */

        private static void VoorraadBijvulling(int artikelnr, int magazijnnr, int aantalstuks)
        {
            using (var entities = new OpleidingenEntities())
            {
                var voorraad = entities.Voorraden.Find(magazijnnr, artikelnr);
                if (voorraad != null)
                {
                    voorraad.AantalStuks += aantalstuks;
                    Console.WriteLine("Pas nu de voorraad aan met de Server Explorer, druk daarna pas op Enter");
                    Console.ReadLine();
                    try
                    {
                        entities.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine("Voorraad werd door andere applicatie aangepast.");
                    }
                }
                else
                {
                    Console.WriteLine("Voorraad niet gevonden");
                }
            }
        }

        private static void VoorraadTransfer(int artikelNr, int vanMagazijnNr, int naarMagazijnNr, int aantalStuks)
        {
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var entities = new OpleidingenEntities())
                {
                    var vanVoorraad = entities.Voorraden.Find(vanMagazijnNr, artikelNr);
                    if (vanVoorraad != null)
                    {
                        if (vanVoorraad.AantalStuks >= aantalStuks)
                        {
                            vanVoorraad.AantalStuks -= aantalStuks;
                            var naarVoorraad = entities.Voorraden.Find(naarMagazijnNr, artikelNr);
                            if (naarVoorraad != null)
                            {
                                naarVoorraad.AantalStuks += aantalStuks;
                            }
                            else
                            {
                                naarVoorraad = new Voorrad
                                {
                                    ArtikelNr = artikelNr,
                                    MagazijnNr = naarMagazijnNr,
                                    AantalStuks = aantalStuks
                                };
                                entities.Voorraden.Add(naarVoorraad);
                            }
                            entities.SaveChanges();
                            transactionScope.Complete();
                        }
                        else
                        {
                            Console.WriteLine("Te weinig voorraad voor transfer");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Artiken niet gevonden in magazijn {0}", vanMagazijnNr);
                    }
                }
            }
        }
    }
}
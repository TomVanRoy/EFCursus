using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Infrastructure;

namespace Opdracht
{
    internal class Program
    {
        private static void AdministratieveKost()
        {
            Console.Write("Met hoeveel moeten alle saldo's verlaagd worden? ");
            decimal bedrag;
            if (decimal.TryParse(Console.ReadLine(), out bedrag))
            {
                using (var entities = new BankEntities())
                {
                    var aantalAangepast = entities.AdministratieveKost(bedrag);
                    Console.WriteLine("{0} rekeningen aangepast", aantalAangepast);
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
        }

        private static void Afbeelden(List<Personeelslid> personeel, int insprong)
        {
            foreach (var personeelslid in personeel)
            {
                Console.Write(new string('\t', insprong));
                Console.WriteLine(personeelslid.Voornaam);
                if (personeelslid.Onderlingen.Count != 0)
                {
                    Afbeelden(personeelslid.Onderlingen.ToList(), insprong + 1);
                }
            }
        }

        private static void Exit()
        {
            Console.WriteLine("Druk op enter om het programma af te sluiten...");
            Console.ReadLine();
        }

        private static List<Klanten> GetKlanten()
        {
            List<Klanten> klanten = new List<Klanten>();
            using (var entities = new BankEntities())
            {
                klanten = entities.Klanten.Include("rekeningen")
                    .OrderBy(klant => klant.Voornaam).ToList();
            }
            return klanten;
        }

        private static void KlantenEnHunRekeningen()
        {
            List<Klanten> klanten = GetKlanten();
            foreach (var klant in klanten)
            {
                var klantNaam = klant.Voornaam;
                Console.WriteLine(new string('-', klantNaam.Length));
                Console.WriteLine(klantNaam);
                Console.WriteLine(new string('-', klantNaam.Length));
                decimal totaal = 0m;
                foreach (var rekening in klant.Rekeningen)
                {
                    Console.WriteLine("{0}: {1}", rekening.RekeningNr, rekening.Saldo);
                    totaal += rekening.Saldo;
                }
                Console.WriteLine("Totaal: " + totaal);
                Console.WriteLine();
            }
        }

        private static void KlantVerwijderen()
        {
            Console.Write("Geef het klantnr van de klant die u wenst te verwijderen . . . ");
            int klantnr;
            if (int.TryParse(Console.ReadLine(), out klantnr))
            {
                using (var entities = new BankEntities())
                {
                    var klant = entities.Klanten.Find(klantnr);
                    if (klant != null)
                    {
                        if (klant.Rekeningen.Count == 0)
                        {
                            entities.Klanten.Remove(klant);
                            entities.SaveChanges();
                            Console.WriteLine("{0} is verwijderd", klant.Voornaam);
                        }
                        else
                        {
                            Console.WriteLine("Klant kan niet verwijderd worden omdat die nog rekening open heeft");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Klant is niet gevonden");
                    }
                }
            }
            else
            {
                Console.WriteLine("Gelieve een geheel getal in te geven");
            }
        }

        private static void KlantWijzigen()
        {
            Console.Write("Geef het klant nummer van de te wijzigen klant . . . ");
            int klantnr;
            if (int.TryParse(Console.ReadLine(), out klantnr))
            {
                using (var entities = new BankEntities())
                {
                    var klant = entities.Klanten.Find(klantnr);
                    if (klant != null)
                    {
                        Console.Write("Geef een nieuwe voornaam voor " + klant.Voornaam + " . . . ");
                        string nieuweNaam = Console.ReadLine();
                        if (nieuweNaam.Trim() != null)
                        {
                            try
                            {
                                klant.Voornaam = nieuweNaam;
                                entities.SaveChanges();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                Console.WriteLine("Een andere gebruiker wijzigde deze klant");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Klant niet gevonden");
                    }
                }
            }
            else
            {
                Console.WriteLine("Gelieve een geheel getal in te geven.");
            }
        }

        private static void Main(string[] args)
        {
            AdministratieveKost();
            Exit();
        }

        private static void Overschrijven()
        {
            var transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead };
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                Console.Write("Geef het rekeningnummer waarvan het geld wordt overgeschreven . . . ");
                using (var entities = new BankEntities())
                {
                    var vanRekeningNr = entities.Rekeningen.Find(Console.ReadLine());
                    if (vanRekeningNr != null)
                    {
                        Console.Write("Geef het rekeningnummer waar het geld naar wordt overgeschreven . . . ");
                        var naarRekeningNr = entities.Rekeningen.Find(Console.ReadLine());
                        if (naarRekeningNr != null)
                        {
                            Console.Write("Geef het bedrag dat je wilt overschrijven . . . ");
                            decimal bedrag;
                            if (decimal.TryParse(Console.ReadLine(), out bedrag))
                            {
                                if (bedrag <= vanRekeningNr.Saldo)
                                {
                                    vanRekeningNr.Saldo -= bedrag;
                                    naarRekeningNr.Saldo += bedrag;
                                    entities.SaveChanges();
                                    transactionScope.Complete();
                                }
                                else
                                {
                                    Console.WriteLine("Het saldo is ontoereikend");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Gelieve een geldig bedrag in te geven.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Rekening kon niet worden gevonden");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Rekening kon niet worden gevonden");
                    }
                }
            }
        }

        private static void Personeel()
        {
            using (var entities = new BankEntities())
            {
                var baas = (from personeel in entities.Personeel
                            where personeel.Manager == null
                            select personeel).ToList();
                Afbeelden(baas, 0);
            }
        }

        private static void Storten()
        {
            Console.WriteLine("Geef het rekeningnummer in . . . ");
            string rekeningnr = Console.ReadLine();
            using (var entities = new BankEntities())
            {
                var rekening = entities.Rekeningen.Find(rekeningnr);
                if (rekening != null)
                {
                    Console.WriteLine("Geef het te storten begrag in . . . ");
                    decimal bedrag;
                    if (decimal.TryParse(Console.ReadLine(), out bedrag))
                    {
                        if (bedrag > 0)
                        {
                            rekening.Saldo += bedrag;
                            entities.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("Tik een positief getal");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tik een getal");
                    }
                }
                else
                {
                    Console.WriteLine("Rekening niet gevonden");
                }
            }
        }

        private static void TotaleSaldoPerKlant()
        {
            using (var entities = new BankEntities())
            {
                foreach (var klant in entities.TotaleSaldoPerKlant.OrderBy(klant => klant.Voornaam))
                {
                    Console.WriteLine("{0}: {1}", klant.Voornaam, klant.TotaleSaldo);
                }
            }
        }

        private static void ZichtrekeningenSpaarrekeningen()
        {
            using (var entities = new BankEntities())
            {
                var query = from rekening in entities.Rekeningen
                            where rekening is Zichtrekening
                            orderby rekening.RekeningNr
                            select rekening;
                foreach (Zichtrekening rekening in query)
                {
                    Console.WriteLine(rekening.RekeningNr + ":  " + rekening.Saldo);
                }
            };
        }

        private static void ZichtrekeningToevoegen()
        {
            int klantnr = 0;
            int maxKlantNr = 0;
            List<Klanten> klanten = GetKlanten();
            foreach (Klanten k in klanten)
            {
                if (k.KlantNr > maxKlantNr)
                {
                    maxKlantNr = k.KlantNr;
                }
                Console.WriteLine("{0}:{1}", k.KlantNr, k.Voornaam);
            }
            Console.WriteLine();
            Console.WriteLine("KlantNr: ");
            if (int.TryParse(Console.ReadLine(), out klantnr))
            {
                if (klantnr > maxKlantNr || klantnr <= 0)
                {
                    Console.WriteLine("Klant niet gevonden");
                }
                else
                {
                    Console.WriteLine("Geef het rekeningnummer van de nieuwe rekening in");
                    var rekeningnr = Console.ReadLine();
                    Rekeningen rekening = new Rekeningen
                    {
                        /*
                        Saldo = 0,
                        RekeningNr = rekeningnr,
                        KlantNr = klantnr,
                        Soort = "Z"
                        */
                    };
                    using (var entities = new BankEntities())
                    {
                        /*
                        entities.Rekeningen.Add(rekening);
                        entities.SaveChanges();
                        */
                    }
                }
            }
            else
            {
                Console.WriteLine("Tik een getal");
            }
        }
    }
}
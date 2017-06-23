using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOpdracht
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<WinkelContext>());
            using (var context = new WinkelContext())
            {
                var videogame = new NonFoodArtikel
                {
                    Artikelgroepen = new Artikelgroep() { Naam = "Video Spel" },
                    Leveranciers = new List<Leverancier>() { new Leverancier { Naam = "Owlcat Games" } },
                    Naam = "Pathfinder: Kingmaker"
                };
                context.Artikels.Add(videogame);
                context.SaveChanges();
            }
        }
    }
}
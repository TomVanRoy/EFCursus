using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstOpdracht
{
    internal class WinkelContext : DbContext
    {
        public DbSet<Leverancier> Leveranciers { get; set; }
        public DbSet<Artikel> Artikels { get; set; }
        public DbSet<Artikelgroep> Artikelgroepen { get; set; }
    }
}
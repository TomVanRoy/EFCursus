using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstCursus
{
    internal class VDABContext : DbContext
    {
        public DbSet<Campus> Campussen { get; set; }
        public DbSet<Cursist> Cursisten { get; set; }
        public DbSet<Cursus> Cursussen { get; set; }
        public DbSet<Instructeur> Instructeurs { get; set; }
        public DbSet<Land> Landen { get; set; }
        public DbSet<Verantwoordelijkheid> Verantoordelijkheden { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<KlassikaleCursus>()
                .Map(m => m.Requires("Soort")
                .HasValue("K"));
            modelBuilder
                .Entity<ZelfstudieCursus>()
                .Map(m => m.Requires("Soort")
                .HasValue("Z"));
            modelBuilder
                .Entity<Instructeur>()
                .HasMany(i => i.Verantwoordelijkheden)
                .WithMany(v => v.Instructeurs)
                .Map(c => c
                        .ToTable("InstructeursVerantwoordelijkheden")
                        .MapLeftKey("VerantwoordelijkheidID")
                        .MapRightKey("InstructeurNr")
                );
        }
    }
}
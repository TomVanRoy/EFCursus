using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFInWPF
{
    public class VDABContext : DbContext
    {
        public DbSet<Cursist> Cursisten { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOpdracht
{
    [Table("Artikels")]
    public abstract class Artikel
    {
        public virtual Artikelgroep Artikelgroepen { get; set; }
        public int ArtikelgroepId { get; set; }
        public int Id { get; set; }
        public ICollection<Leverancier> Leveranciers { get; set; }
        public string Naam { get; set; }
    }
}
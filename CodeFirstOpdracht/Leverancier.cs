using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOpdracht
{
    [Table("Leveranciers")]
    public class Leverancier
    {
        public ICollection<Artikel> Artikels { get; set; }
        public int Id { get; set; }
        public string Naam { get; set; }
    }
}
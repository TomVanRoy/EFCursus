using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOpdracht
{
    [Table("FoodArtikels")]
    public class FoodArtikel : Artikel
    {
        public int Houdbaarheid { get; set; }
    }
}
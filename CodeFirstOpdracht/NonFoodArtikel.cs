using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstOpdracht
{
    [Table("NonFoodArtikels")]
    public class NonFoodArtikel : Artikel
    {
        public int Garantie { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CodeFirstCursus
{
    [Table("Verantwoordelijkheden")]
    public class Verantwoordelijkheid
    {
        public virtual ICollection<Instructeur> Instructeurs { get; set; }
        public string Naam { get; set; }
        public int VerantwoordelijkheidId { get; set; }
    }
}
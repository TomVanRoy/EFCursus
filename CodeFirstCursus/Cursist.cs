using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{
    [Table("Cursisten")]
    public class Cursist
    {
        public virtual ICollection<Cursist> Beschermelingen { get; set; }

        [Key]
        public int CursistId { get; set; }

        public string Familienaam { get; set; }

        [InverseProperty("Beschermelingen"), ForeignKey("MentorId")]
        public virtual Cursist Mentor { get; set; }

        public int? MentorId { get; set; }

        public string Voornaam { get; set; }
    }
}
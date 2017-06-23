using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstCursus
{
    [Table("Zelfstudiecursussen")]
    public class ZelfstudieCursus : Cursus
    {
        public int AantalDagen { get; set; }
    }
}
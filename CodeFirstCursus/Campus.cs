using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CodeFirstCursus
{
    [Table("Campussen")]
    internal class Campus
    {
        public int CampusId { get; set; }

        [Required, StringLength(50)]
        public string Naam { get; set; }
    }
}
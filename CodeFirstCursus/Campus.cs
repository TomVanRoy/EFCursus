﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CodeFirstCursus
{
    [Table("Campussen")]
    public class Campus
    {
        public Adres Adres { get; set; }
        public int CampusId { get; set; }

        public virtual ICollection<Instructeur> Instructeurs { get; set; }

        [Required, StringLength(50)]
        public string Naam { get; set; }
    }
}
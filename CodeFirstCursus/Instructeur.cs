﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CodeFirstCursus
{
    public class Instructeur
    {
        public Adres Adres { get; set; }
        public virtual Campus Campus { get; set; }
        public int CampusId { get; set; }
        public string Familienaam { get; set; }
        public bool? HeeftRijbewijs { get; set; }

        [Column(TypeName = "date")]
        public DateTime InDienst { get; set; }

        [Key]
        public int InstructeurNr { get; set; }

        public virtual ICollection<Verantwoordelijkheid> Verantwoordelijkheden { get; set; }
        public string Voornaam { get; set; }

        [Column("maandwedde")]
        public decimal Wedde { get; set; }

        public void Opslag(decimal percentage)
        {
            Wedde *= (1m + percentage / 100m);
        }
    }
}
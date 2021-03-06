﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFInWPF
{
    [Table("Cursisten")]
    public class Cursist
    {
        public virtual ObservableCollection<Cursist> Beschermelingen { get; set; }

        [Key]
        public int CursistNr { get; set; }

        public string Familienaam { get; set; }

        [InverseProperty("Beschermelingen")]
        [ForeignKey("MentorNr")]
        public virtual Cursist Mentor { get; set; }

        public int? MentorNr { get; set; }

        public string Naam
        {
            get
            {
                return Voornaam + ' ' + Familienaam;
            }
        }

        public string Voornaam { get; set; }
    }
}
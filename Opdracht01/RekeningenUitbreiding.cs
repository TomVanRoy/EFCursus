using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht
{
    public partial class Rekeningen
    {
        public void Storten(decimal bedrag)
        {
            Saldo += bedrag;
        }
    }
}
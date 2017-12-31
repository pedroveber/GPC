using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{
  public  class DefesaSiegeModels:AtaquesSiegeModels
    {
        public int TotalAtaquesRecebidos { get { return this.Derrota + this.Vitoria; } }
    }
    
}

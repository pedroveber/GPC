using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GPCLib.Models
{
  public class AtaquesSiegeModels
    {
        public SiegeModels Siege { get; set; }
        public PlayerModels Player { get; set; }
        public int Vitoria { get; set; }
        public int Derrota { get; set; }
        public int MonstrosNaoUsados { get; set; }
    }
}

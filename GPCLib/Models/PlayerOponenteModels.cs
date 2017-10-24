using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{
    public class PlayerOponenteModels
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Bonus { get; set; }

        public TimeDefesaModels TimeDefesa { get; set; }

        public List<LutasModels> Lutas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{
    public class LutasModels
    {
        public int Id { get; set; }
        public BatalhaModels Batalhas { get; set; }

        public PlayerModels Player { get; set; }
        public PlayerOponenteModels PlayerOponente { get; set; }

        public int Vitoria { get; set; }
        public int ValorBarra { get; set; }
        public DateTime DataHora { get; set; }
        public string MomentoVitoria { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{

    public class ResultadoGeralModels
    {
        public BatalhaModels Batalha { get; set; }
        public List<ResultadoGeralItensModels> Item { get; set; }

    }

    public class ResultadoGeralItensModels
    {
        public PlayerModels Player { get; set; }
        public int Bonus { get; set; }
        public int NumeroLutas { get; set; }
        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }

    }

    
}

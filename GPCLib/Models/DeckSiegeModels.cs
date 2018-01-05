using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{
    public class DeckSiegeModels
    {
        public long Id { get; set; }
        public long IdDeck { get; set; }

        public SiegeModels Siege { get; set; }

        public PlayerModels Player { get; set; }

        public MonstroModels Monstro1 { get; set; }
        public MonstroModels Monstro2 { get; set; }
        public MonstroModels Monstro3 { get; set; }

        public int Vitoria { get; set; }
        public int Derrota { get; set; }
        public int AtaquesRecebidos { get; set; }

        public string BasesDefendidas { get; set; }
    }
}

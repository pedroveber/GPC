using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPCLib.Models
{
    public class GuildaModels
    {
        public long Id { get; set; }
        public string Nome { get; set; }
    }

    public class GuildaPlayer
    {
        public long idPlayer { get; set; }
        public string idUsuario { get; set; }
        public long idGuilda { get; set; }
        public int Ativo { get; set; }
    }
}

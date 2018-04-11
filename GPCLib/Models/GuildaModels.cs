using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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


    public class ResumoGuildaModels
    {
        public GuildaModels Guilda { get; set; }
        public int QtdGVGs { get; set; }
        public int VitoriaGvg { get; set; }
        public int DerrotaGVG { get; set; }
        public int Siege { get; set; }

        public int SiegePosicao1 { get; set; }
        public int SiegePosicao2 { get; set; }
        public int SiegePosicao3 { get; set; }

    }

    public class GuildaComboModels
    {
        public string SelectedOption { get; set; }

        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }

}

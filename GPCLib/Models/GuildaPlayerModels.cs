using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GPCLib.Models
{
    public class GuildaPlayersModels
    {
        public GuildaModels Guilda { get; set; }
        public List<UsuarioModels> Usuarios { get; set; }
        public List<PlayerModels> Players { get; set; }

        public GuildaPlayerListModels GuildaUsuarioListBox { get; set; }
        public GuildaPlayerListModels TodosPlayersListBox { get; set; }

    }

    public class GuildaPlayerListModels
    {
        public int[] idPlayer { get; set; }
        public IEnumerable<SelectListItem> Players { get; set; }

    }

    public class TodosPlayersList
    {
        public int[] idPlayer { get; set; }
        public IEnumerable<SelectListItem> Players { get; set; }
    }

    public class PlayerUsuarioModels
    {
        public GuildaModels Guilda { get; set; }
        public UsuarioModels Usuario { get; set; }
        public PlayerModels Player { get; set; }

        public bool Ativo { get; set; }

        public UsuarioCombo UsuarioCombo { get; set; }

    }
}

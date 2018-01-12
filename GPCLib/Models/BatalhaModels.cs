using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GPCLib.Models
{
    public class BatalhaModels
    {
        public long Id { get; set; }
        public long IdGuildaOponente { get; set; }
        public string GuildaOponente { get; set; }
        public int Life { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int PontuacaoOponente { get; set; }
        public int PontuacaoGuild { get; set; }
        public int RankGuild { get; set; }
        public GuildaModels GuildaAtacante { get; set; }
        public bool Vitoria { get; set; }
        public int QuantidadeAtaques { get; set; }
    }
}

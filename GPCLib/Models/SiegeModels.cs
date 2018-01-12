using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GPCLib.Models
{
    public class SiegeModels
    {
        public long Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int IdGuildaUsuarioLogado { get; set; }
        public List<SiegeGuildaModels> Guilda { get; set; }

    }

    public class SiegeGuildaModels
    {
        public GuildaModels Guilda { get; set; }
        public int Posicao { get; set; }
        public int Rating { get; set; }
        public double MatchScore { get; set; }
        public int Members { get; set; }
        public string ImagemRating
        {
            get
            {
                switch (this.Rating)
                {
                    case 4001:
                        return "guardian1.png";
                    case 4002:
                        return "guardian2.png";
                    case 4003:
                        return "guardian3.png";
                    case 3001:
                        return "conqueror1.png";
                    case 3002:
                        return "conqueror2.png";
                    case 3003:
                        return "conqueror3.png";
                    case 2001:
                        return "fighter1.png";
                    case 2002:
                        return "fighter2.png";
                    case 2003:
                        return "fighter3.png";
                    default:
                        return "xx";

                }

                ;
            }

        }

    }
}

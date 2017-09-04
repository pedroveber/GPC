using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GPCLib.Models
{
    public class DefesaSemanaModels
    {
        public List<DefesaModels> Defesas { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataFim { get; set; }


    }

    public class DefesaModels
    {
        public PlayerModels Player { get; set; }

        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }

        public int AtaquesSofridos
        {
            get
            {
                return this.Vitoria + this.Derrota + this.Empate;
            }

        }
    }
    public class TimeDefesaModels
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }

        public MonstroModels Monstro1 { get; set; }
        public MonstroModels Monstro2 { get; set; }
        public MonstroModels Monstro3 { get; set; }
        public MonstroModels Monstro4 { get; set; }
        public MonstroModels Monstro5 { get; set; }
        public MonstroModels Monstro6 { get; set; }

    }
    
}

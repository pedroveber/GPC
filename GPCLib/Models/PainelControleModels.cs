using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPCLib.Models
{
    public class PainelControleModels
    {
        public List<PainelControleAtaquesModels> Ataques { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataFim { get; set; }
    }
    public class PainelControleAtaquesModels
    {
        public PlayerModels Player { get; set; }
        public int Escalado { get; set; }
        public int NAtacou { get; set; }
        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }
       
    }
}

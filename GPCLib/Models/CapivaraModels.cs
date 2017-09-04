using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GPCLib.Models
{
    public class CapivaraModels
    {
        public PlayerModels Player { get; set; }
        public List<DefesasPlayerConsolidado> DefesasConsolidado { get; set; }

        public List<AtaquesPlayerConsolidado> AtaquesConsolidado { get; set; }

        public List<AtaquesSemana> AtaquesSemana { get; set; }

        public TimeDefesaModels TimeGVG { get; set; }

        public double PercentualVitoria { get; set; }
        public double PercentualDefesa { get; set; }

        public double PercentualParticipacao { get; set; }

        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }

        public int DefesaVitorias { get; set; }
        public int DefesaEmpates { get; set; }
        public int DefesaDerrotas { get; set; }

        public string Imagem { get; set; }

        public int Escalado { get; set; }
        public int NAtacou { get; set; }

        public int StreakVitoria { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InicioStreak { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FimStreak { get; set; }

    }

    public class AtaquesPlayerConsolidado
    {
        public string Guilda { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }

    }

    public class DefesasPlayerConsolidado
    {
        public string NomeGuild { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }


    }

    public class DefesasPlayer
    {
        public PlayerModels Player { get; set; }
        public string NomeGuild { get; set; }
        public string NomeOponente { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        public int Resultado { get; set; }


    }
}

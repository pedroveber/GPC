using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib.Models;
using GPCLib.DataAccess;

namespace WebApplication1.Controllers
{
    public class CapivaraController : Controller
    {
        // GET: Capivara
        public ActionResult Index(int id)
        {
            CapivaraModels Capivara = new CapivaraModels();


            //ObterPlayer
            Capivara = new Player().ObterPlayerCapivara(id);

            if (Capivara.Player != null)
            {
                //Obter a segunda da semana passada
                DateTime segundaFeira = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);

                //Defesas da Semana
                Capivara.DefesasConsolidado = new GPCLib.DataAccess.DefesaPlayer().ListarDefesaConsolidado(segundaFeira.AddDays(-7), segundaFeira.AddDays(-1), id);

                GPCLib.DataAccess.AtaquesPlayer daAtaquesPlayer = new AtaquesPlayer();
                //Ataques da Semana
                Capivara.AtaquesConsolidado = daAtaquesPlayer.ListarAtaqueConsolidado(segundaFeira, segundaFeira.AddDays(6), id);
                
                Capivara = CalcularStreak(new AtaquesPlayer().ListarAtaques(id), Capivara);


            }
            else
            {
                Capivara.Player = new PlayerModels();
                Capivara.AtaquesConsolidado = new List<AtaquesPlayerConsolidado>();
                Capivara.DefesasConsolidado = new List<DefesasPlayerConsolidado>();

            }

            return View(Capivara);
        }

        [HttpPost]
        public ActionResult Index(string ddlPlayer)
        {
            CapivaraModels Capivara = new CapivaraModels();
            int id = Convert.ToInt32(Request.Form[0]);

            //ObterPlayer
            Capivara = new Player().ObterPlayerCapivara(id);

            //Obter a segunda da semana passada
            DateTime segundaFeira = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);

            //Defesas da Semana
            Capivara.DefesasConsolidado = new GPCLib.DataAccess.DefesaPlayer().ListarDefesaConsolidado(segundaFeira.AddDays(-7), segundaFeira.AddDays(-1), id);

            GPCLib.DataAccess.AtaquesPlayer daAtaquesPlayer = new AtaquesPlayer();
            //Ataques da Semana
            Capivara.AtaquesConsolidado = daAtaquesPlayer.ListarAtaqueConsolidado(segundaFeira, segundaFeira.AddDays(6), id);
                        
            Capivara = CalcularStreak(new AtaquesPlayer().ListarAtaques(id), Capivara);

            return View(Capivara);
        }

        #region Calcular Streak Vitórias




        private CapivaraModels CalcularStreak(List<GPCLib.Models.LutasModels> Lutas, CapivaraModels capi)
        {
            List<LutasModels> LutasOrdenado = Lutas.OrderBy(o => o.DataHora).ToList();

            int countStreak = 0;
            int vitoriaAnterior = 0;

            int countMaiorStreak = 0;
            DateTime dInicioMaiorStreak = DateTime.MinValue;
            DateTime dFimMaiorStreak = DateTime.MinValue;

            DateTime dInicioStreak = DateTime.MinValue;
            DateTime dFimStreak = DateTime.MinValue;

            //se o indice 2 já for uma vitoria, guarda a da inicio e a vitorianAnterir
            if (Lutas[0].Vitoria == 2)
            {
                dInicioStreak = Lutas[0].DataHora;
                vitoriaAnterior = 2;
            }

            foreach (LutasModels item in LutasOrdenado)
            {

                if (item.Vitoria == 2)
                {
                    //Se o loop anterior for diretente de vitoria tem que resetar a data inicio.
                    if (vitoriaAnterior != 2)
                    {
                        dInicioStreak = item.DataHora;
                    }
                    countStreak++;
                    dFimStreak = item.DataHora;

                }
                else
                {
                    //guardar o maior streak
                    if (countStreak >= countMaiorStreak && countStreak > 0)
                    {
                        dInicioMaiorStreak = dInicioStreak;
                        dFimMaiorStreak = dFimStreak;
                        countMaiorStreak = countStreak;
                    }


                    countStreak = 0;
                    dFimStreak = DateTime.MinValue;
                }

                vitoriaAnterior = item.Vitoria;
            }

            //guardar o maior streak
            if (countStreak >= countMaiorStreak && countStreak > 0)
            {
                dInicioMaiorStreak = dInicioStreak;
                dFimMaiorStreak = dFimStreak;
                countMaiorStreak = countStreak;
            }

            capi.FimStreak = dFimMaiorStreak;
            capi.InicioStreak = dInicioMaiorStreak;
            capi.StreakVitoria = countMaiorStreak;

            return capi;

        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class PainelControleController : Controller
    {
        // GET: PainelControle
        public ActionResult Index()
        {
            
            //Obter a segunda desta semana
            DateTime segundaFeira = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);

            GPCLib.Models.PainelControleModels retorno = new GPCLib.Models.PainelControleModels();
            retorno = new GPCLib.DataAccess.PainelControle().ListaConsolidada(segundaFeira, segundaFeira.AddDays(6));
            
            return View(retorno);

        }

        [HttpPost]
        public ActionResult filtro(string txtDataInicio, string txtDataFim)
        {
            DateTime inicioSemana;
            DateTime fimSemana;

            inicioSemana = Convert.ToDateTime(txtDataInicio);
            fimSemana = Convert.ToDateTime(txtDataFim);

            
            return View("index",new GPCLib.DataAccess.PainelControle().ListaConsolidada(inicioSemana, fimSemana));
        }
    }
}
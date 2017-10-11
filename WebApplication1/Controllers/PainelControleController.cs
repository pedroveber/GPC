using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using App.Extensions;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PainelControleController : Controller
    {
        // GET: PainelControle
        public ActionResult Index()
        {
            
            //Obter a segunda desta semana
            DateTime segundaFeira = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);

            long idGuilda=0;
            long.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            GPCLib.Models.PainelControleModels retorno = new GPCLib.Models.PainelControleModels();
            retorno = new GPCLib.DataAccess.PainelControle().ListaConsolidada(segundaFeira, segundaFeira.AddDays(6),idGuilda );
            
            return View(retorno);

        }

        [HttpPost]
        public ActionResult filtro(string txtDataInicio, string txtDataFim)
        {
            DateTime inicioSemana;
            DateTime fimSemana;

            long idGuilda = 0;
            long.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            inicioSemana = Convert.ToDateTime(txtDataInicio);
            fimSemana = Convert.ToDateTime(txtDataFim);

            
            return View("index",new GPCLib.DataAccess.PainelControle().ListaConsolidada(inicioSemana, fimSemana, idGuilda));
        }
    }
}
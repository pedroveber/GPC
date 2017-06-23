using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DefesasSemanaController : Controller
    {
        // GET: DefesasSemana
        public ActionResult Index()
        {
            //Obter a segunda da semana passada
            DateTime segundaFeira = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);

            GPCLib.Models.DefesaSemanaModels retorno = new GPCLib.Models.DefesaSemanaModels();
            retorno = new GPCLib.DataAccess.DefesaPlayer().ListarDefesasSemana(segundaFeira, segundaFeira.AddDays(6));
            
            return View(retorno);
        }

        [HttpPost]
        public ActionResult filtro(string txtDataInicio, string txtDataFim)
        {
            DateTime inicioSemana;
            DateTime fimSemana;

            inicioSemana = Convert.ToDateTime(txtDataInicio);
            fimSemana = Convert.ToDateTime(txtDataFim);

            return View("index", new GPCLib.DataAccess.DefesaPlayer().ListarDefesasSemana(inicioSemana,fimSemana));
        }
    }
}
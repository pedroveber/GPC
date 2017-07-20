using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib.Models;
using GPCLib.DataAccess;

namespace WebApplication1.Controllers

{
    public class AtaquesSemanaController : Controller
    {

        public ActionResult Index(int id)
        {
            GPCLib.DataAccess.AtaquesPlayer daAtaquesPlayer = new AtaquesPlayer();
            List<AtaquesSemana> ataques = new List<AtaquesSemana>();
            ataques = daAtaquesPlayer.ListarAtaquesPorSemana(id);
            return View(ataques);
        }

        // GET: AtaquesSemana
        [HttpPost]
        public ActionResult ListarAtaquesSemana(int id)
        {
            try
            {
                GPCLib.DataAccess.AtaquesPlayer daAtaquesPlayer = new AtaquesPlayer();
                //Obter Ataques Totais (Gráfico)a
                List<AtaquesSemana> ataques = new List<AtaquesSemana>();
                ataques = daAtaquesPlayer.ListarAtaquesPorSemana(id);

                return Json(ataques.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // GET: AtaquesSemana
        [HttpPost]
        public ActionResult ListarDefesasSemana(int id)
        {
            try
            {
                GPCLib.DataAccess.DefesaPlayer daDefesasPlayer = new DefesaPlayer();
                
                //Obter Ataques Totais (Gráfico)
                List<DefesasSemana> defesas = new List<DefesasSemana>();
                defesas = daDefesasPlayer.ListarDefesasPorSemana(id);

                return Json(defesas.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}   
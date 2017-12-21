using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib.Models;
using GPCLib.DataAccess;
using Microsoft.AspNet.Identity;
using App.Extensions;


namespace WebApplication1.Controllers
{
    public class SiegeController : Controller
    {
        // GET: Siege
        public ActionResult ListarSieges()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();

            List<SiegeModels> lstSiege = new List<SiegeModels>();
            lstSiege= daSiege.ListarSieges(idGuilda);
            lstSiege = lstSiege.OrderByDescending(x => x.Data).ToList();

            return View(lstSiege);
        }

        public ActionResult ListarAtaquesConsolidado(long idSiege)
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<AtaquesSiegeModels> lstAtaques = new List<AtaquesSiegeModels>();
            lstAtaques = daSiege.ListarAtaquesConsolidado(idGuilda, idSiege);

            return View(lstAtaques);
        }
    }
}
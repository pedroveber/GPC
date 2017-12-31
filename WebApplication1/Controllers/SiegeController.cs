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
    [Authorize]
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

        public ActionResult ListarDefesasConsolidado(long idSiege)
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<DefesaSiegeModels> lstDefesas = new List<DefesaSiegeModels>();
            lstDefesas = daSiege.ListarDefesasConsolidado(idGuilda, idSiege);

            return View(lstDefesas);
        }

        public ActionResult ListarDeckPlayer(long idSiege, int idPlayer)
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<DeckSiegeModels> lstDecks = new List<DeckSiegeModels>();
            lstDecks = daSiege.ListarDecksPlayer(idGuilda, idSiege, idPlayer);
            lstDecks = lstDecks.OrderByDescending(x => x.Vitoria).ToList();

            return View(lstDecks);
        }
        public ActionResult ListarVitoriasTimes()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<DeckSiegeModels> lstDecks = new List<DeckSiegeModels>();
            lstDecks = daSiege.ListarVitoriasTimes(idGuilda);
            return View(lstDecks);
        }
        

        public ActionResult ListarAtaquesGeral()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<AtaquesSiegeModels> lstAtaques = new List<AtaquesSiegeModels>();
            lstAtaques = daSiege.ListarAtaques(idGuilda);

            return View(lstAtaques);
        }

        public ActionResult ListarDefesasGeral()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Siege daSiege = new Siege();
            List<DefesaSiegeModels> lstDefesas = new List<DefesaSiegeModels>();
            lstDefesas = daSiege.ListarDefesas(idGuilda);

            return View(lstDefesas);
        }
    }
}
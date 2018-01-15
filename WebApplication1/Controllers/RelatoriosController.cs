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
{[Authorize]
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        public ActionResult ResultadoGeral(int id)
        {
            Relatorios daRelatorios = new Relatorios();
            Batalha daBatalha = new Batalha();

            ResultadoGeralModels retorno = new ResultadoGeralModels();
            retorno.Batalha = daBatalha.ObterBatalha(id);
            retorno.Item = daRelatorios.ListarResultadoGeral(id);

            retorno.Item = retorno.Item.OrderBy(x => x.Player.Nome).ToList();
            return View(retorno);
        }

        public ActionResult ListaBatalhas()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Batalha daBatalha = new Batalha();
            List<BatalhaModels> lstBatalhas = new List<BatalhaModels>();
            lstBatalhas = daBatalha.ListarBatalhas(idGuilda, false);
            lstBatalhas = lstBatalhas.OrderByDescending(x => x.Data).ToList();
            return View(lstBatalhas);
        }

        public ActionResult ListaBatalhasTodas()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Batalha daBatalha = new Batalha();
            List<BatalhaModels> lstBatalhas = new List<BatalhaModels>();
            lstBatalhas = daBatalha.ListarBatalhas(idGuilda, true);
            lstBatalhas = lstBatalhas.OrderByDescending(x => x.Data).ToList();
            
            return View("ListaBatalhas", lstBatalhas);
        }

        public ActionResult ListaDefesasGVG(int id)
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            GPCLib.Models.TorresGVG objRetorno = new TorresGVG();
            objRetorno.Oponente = new GPCLib.DataAccess.Player().ListarDefesasGVGOponente(id);

            //Obter as Lutas e Filtrar
            List<LutasModels> lstLutas = new GPCLib.DataAccess.Luta().ListarLutas(id, idGuilda);

            objRetorno.Batalha = new GPCLib.DataAccess.Batalha().ObterBatalha(id);

            foreach (PlayerOponenteModels item in objRetorno.Oponente)
            {
                //Obter as Lutas
                item.Lutas = lstLutas.FindAll(x => x.PlayerOponente.Id == item.Id);
            }
            

            return View(objRetorno);
        }

        public ActionResult ListarDefesasGVGConsolidado()
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            List<TimeDefesaConsolidadoModels> objRetorno = new List<TimeDefesaConsolidadoModels>();
            objRetorno = new GPCLib.DataAccess.Relatorios().ListarDefesasGVGConsolidado(7640372);

            return View(objRetorno);
        }

        [HttpGet]
        public JsonResult ObterResumo(int id)
        {
            int idGuilda = 0;
            int.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            Relatorios daRelatorio = new Relatorios();

            ResumoGuildaModels objResumo = new ResumoGuildaModels();
            objResumo.Guilda = new GuildaModels();
            objResumo = daRelatorio.ListarResumoGuilda(idGuilda, id);

            return Json(objResumo, JsonRequestBehavior.AllowGet);
        }

        
    }

}
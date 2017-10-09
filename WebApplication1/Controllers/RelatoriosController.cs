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
            Batalha daBatalha = new Batalha();
            List<BatalhaModels> lstBatalhas = new List<BatalhaModels>();
            lstBatalhas = daBatalha.ListarBatalhas(int.Parse(User.Identity.GetIdGuilda()),false);
            lstBatalhas = lstBatalhas.OrderByDescending(x => x.Data).ToList();
            return View(lstBatalhas);
        }

        public ActionResult ListaBatalhasTodas()
        {
            Batalha daBatalha = new Batalha();
            List<BatalhaModels> lstBatalhas = new List<BatalhaModels>();
            lstBatalhas = daBatalha.ListarBatalhas(int.Parse(User.Identity.GetIdGuilda()), true);
            lstBatalhas = lstBatalhas.OrderByDescending(x => x.Data).ToList();
            //todo: Listar só a guild do usuario logado
            return View("ListaBatalhas", lstBatalhas);
        }
    }

}
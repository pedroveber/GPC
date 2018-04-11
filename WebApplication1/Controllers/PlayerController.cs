using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib;
using Microsoft.AspNet.Identity;
using App.Extensions;

namespace WebApplication1.Controllers
{
    
    public class PlayerController : Controller
    {
        // GET: Player/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GPCLib.DataAccess.Player dPlayer = new GPCLib.DataAccess.Player();

            return View(dPlayer.ObterPlayer(7640372));
        }

        [Authorize]
        public ActionResult ComboPlayers(int id,bool incluirTodos)
        {
            long idGuilda = 0;
            long.TryParse(User.Identity.GetIdGuilda(), out idGuilda);

            List<GPCLib.Models.PlayerModels> players = new List<GPCLib.Models.PlayerModels>();
            players = new GPCLib.DataAccess.Player().ListarPlayers(idGuilda);

            if (incluirTodos)
            {
                players.Insert(0, new GPCLib.Models.PlayerModels { Id = 0, Nome = "TODOS" });
            }
            

            var model = new GPCLib.Models.PlayerComboModels();
            model.SelectOptions = players.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            }).ToList();

            model.SelectedOption = id.ToString();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult ListarPlayersSemUsuarios(long idGuilda)
        {
            List<GPCLib.Models.PlayerModels> players = new List<GPCLib.Models.PlayerModels>();
            players = new GPCLib.DataAccess.Player().ListarPlayersSemUsuarios(idGuilda);

            if (players.Count() ==0)
            {
                players.Add(new GPCLib.Models.PlayerModels() { Id = 0, Nome = "Nenhum player cadastrado..." });
                    
            }
            
            var model = new GPCLib.Models.PlayerComboModels();
            model.SelectOptions = players.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            }).ToList();

            return Json(model);
        }



    }
}

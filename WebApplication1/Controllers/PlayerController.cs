using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib;

namespace WebApplication1.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            GPCLib.DataAccess.Player dPlayer = new GPCLib.DataAccess.Player();

            return View(dPlayer.ObterPlayer(7640372));
        }

        public ActionResult ComboPlayers()
        {
            List<GPCLib.Models.PlayerModels> players = new List<GPCLib.Models.PlayerModels>();
            players = new GPCLib.DataAccess.Player().ListarPlayers();

            var model = new GPCLib.Models.PlayerComboModels();
            model.SelectOptions = players.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            }).ToList();

            return PartialView(model);
        }



    }
}

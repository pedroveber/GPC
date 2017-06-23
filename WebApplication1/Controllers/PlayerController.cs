using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
              
        
    }
}

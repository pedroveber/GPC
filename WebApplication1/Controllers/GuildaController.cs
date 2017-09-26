using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class GuildaController : Controller
    {
        // GET: Guilda
        public ActionResult Index()
        {
            GPCLib.DataAccess.Guilda daGuilda = new GPCLib.DataAccess.Guilda();
            return View(daGuilda.ListarGuildas());
        }
        public ActionResult GuildaPlayer(int id)
        {
            
            GPCLib.DataAccess.Guilda daGuilda = new GPCLib.DataAccess.Guilda();

            List<SelectListItem> listSelectListItem = new List<SelectListItem>();
            List<SelectListItem> listSelectListItemTodos = new List<SelectListItem>();

            //Lista os Players para ambos ListBox
            GPCLib.Models.GuildaPlayerModels listGuilda = daGuilda.ListarUsuariosGuilda(id);
            List<GPCLib.Models.PlayerModels> PlayersTodos  = new GPCLib.DataAccess.Player().ListarPlayers();

            //Monta o Objeto da Listbox
            foreach (GPCLib.Models.PlayerModels item in listGuilda.Players)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Id.ToString()
                };
                listSelectListItem.Add(selectList);
            }

            //Monta o Objeto da Listbox
            foreach (GPCLib.Models.PlayerModels item in PlayersTodos)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Id.ToString()
                };
                listSelectListItemTodos.Add(selectList);
            }
            
            //Atribui ao objeto de retorno
            GPCLib.Models.GuildaPlayerListModels listModel = new GPCLib.Models.GuildaPlayerListModels();
            listModel.Players = listSelectListItem;

            GPCLib.Models.GuildaPlayerListModels listModelTodos = new GPCLib.Models.GuildaPlayerListModels();
            listModelTodos.Players = listSelectListItemTodos;


            listGuilda.TodosPlayersListBox = listModelTodos;
            listGuilda.GuildaUsuarioListBox = listModel;

            return View(listGuilda);
        }

        [HttpPost]
        public ActionResult GravarUsuariosGuilda(IEnumerable<string> idPlayer)
        {
            //Excluir todos da GUild_player e depois incluir novamente. 

            string teste = "asdasds";
            return View();
        }
    }
}
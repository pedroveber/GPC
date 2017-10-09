using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPCLib;

namespace WebApplication1.Controllers
{
    [Authorize]
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
            GPCLib.Models.GuildaPlayersModels listGuilda = daGuilda.ListarUsuariosGuilda(id);
            List<GPCLib.Models.PlayerModels> PlayersTodos  = new GPCLib.DataAccess.Player().ListarPlayersSemGuild();

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
        public ActionResult GravarUsuariosGuilda(GPCLib.Models.GuildaPlayersModels model)
        {
            

            GPCLib.DataAccess.Guilda daGuilda = new GPCLib.DataAccess.Guilda();
            GPCLib.DataAccess.Player daPlayer = new GPCLib.DataAccess.Player();
            GPCLib.DataAccess.Usuario daUsuario = new GPCLib.DataAccess.Usuario();
            GPCLib.Models.PlayerUsuarioModels playerUsuario;

            //Excluir membros da GUild e inserir novamente. 
            daGuilda.ExcluirMembrosGuilda(model.Guilda.Id);
            //TODO: Quando exclui os membros perde o ID do Usuario, corrigir.

            GPCLib.Models.GuildaPlayer objPlayer;

            foreach (int item in model.GuildaUsuarioListBox.idPlayer)
            {
                objPlayer = new GPCLib.Models.GuildaPlayer();
                objPlayer.Ativo = 1;
                objPlayer.idGuilda = model.Guilda.Id;
                objPlayer.idPlayer = item;
                daGuilda.InserirMembroGuilda(objPlayer);

                //Se já tiver Usuario x Player já grava a Guilda no ASPNETUSERs.
                playerUsuario = daPlayer.ObterPlayerUsuario(item);
                if (playerUsuario.Usuario!=null)
                {
                    //Atualiza o codigo da Guilda
                    daUsuario.AtualizarCodGuilda(model.Guilda.Id, playerUsuario.Usuario.Id);
                    
                }
                

            }
            TempData["Success"] = "Gravado com sucesso";
            return RedirectToAction("GuildaPlayer", new { id = model.Guilda.Id});
            
        }
        public ActionResult UsuarioPlayer()
        {
            GPCLib.DataAccess.Player daPlayer = new GPCLib.DataAccess.Player();
            


            List<GPCLib.Models.PlayerUsuarioModels> lstPlayersUsuarios = new List<GPCLib.Models.PlayerUsuarioModels>();
            
            

            lstPlayersUsuarios = daPlayer.ListarPlayerUsuario();
                        
            return View(lstPlayersUsuarios);

        }

        public ActionResult EditUsuarioPlayer(int id)
        {
            //Listar todos Usuários
            GPCLib.Models.PlayerUsuarioModels objPlayerUsuario = new GPCLib.Models.PlayerUsuarioModels();
            List<GPCLib.Models.UsuarioModels> lstUsuarios = new List<GPCLib.Models.UsuarioModels>();
            GPCLib.DataAccess.Player daPlayer = new GPCLib.DataAccess.Player();
            GPCLib.DataAccess.Usuario daUsuario = new GPCLib.DataAccess.Usuario();

            objPlayerUsuario = daPlayer.ObterPlayerUsuario(id);
            lstUsuarios = daUsuario.ListarUsuarios();

            objPlayerUsuario.UsuarioCombo = new GPCLib.Models.UsuarioCombo();

            lstUsuarios.Insert(0,new GPCLib.Models.UsuarioModels() {Id="",UserName="Selecione" });
            objPlayerUsuario.UsuarioCombo.SelectOptions = lstUsuarios.Select(x => new SelectListItem {
                Value = x.Id.ToString(),
                Text = x.UserName
            }).ToList();


            if (objPlayerUsuario.Usuario != null)
            {
                objPlayerUsuario.UsuarioCombo.SelectedOption = objPlayerUsuario.Usuario.Id;
            }
                
            return View(objPlayerUsuario);

        }

        public ActionResult GravarUsuariosPlayer(GPCLib.Models.PlayerUsuarioModels model)
        {
            GPCLib.DataAccess.Guilda daGuilda = new GPCLib.DataAccess.Guilda();
            daGuilda.AtualizarPlayerUsuario(model);
            TempData["Success"] = "Gravado com sucesso";
            return RedirectToAction("EditUsuarioPlayer", new { id = model.Player.Id });
                    
        }
    }
}
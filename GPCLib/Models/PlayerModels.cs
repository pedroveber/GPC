using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace GPCLib.Models
{
    public class PlayerModels
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Level { get; set; }
        public int PontoArena { get; set; }
        public bool Ativo { get; set; }

        public UsuarioModels Usuario { get; set; }

    }
    public class PlayerComboModels
    {
        public string SelectedOption { get; set; }

        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
    

}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GPCLib.Models
{
    public class UsuarioModels
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public class UsuarioCombo
    {
        public string SelectedOption { get; set; }

        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
}

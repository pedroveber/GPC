using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GPCLib.Models
{
    public class PlayerModels
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Level { get; set; }
        public int PontoArena { get; set; }
        public bool Ativo { get; set; }
               
       
     }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSecurity.Model.Menu.Entities
{
    public class MenuEntities
    {
        public int MenuId { get; set; }
        public int MenuRol { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public string MenuUrl { get; set; }
        public int MenuPosition { get; set; }
        public string MenuStatus { get; set; }
        public int? MenuFatherId { get; set; }
         
    }
}

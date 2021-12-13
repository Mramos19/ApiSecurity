using ApiSecurity.Model.Menu.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSecurity.Service.Menu
{
    public interface IMenuServices
    {
        public List<MenuEntities> GetMenuByRolId(int RolId);
    }
}

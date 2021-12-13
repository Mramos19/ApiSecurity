using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiSecurity.Data;
using ApiSecurity.Model.Menu.Entities;

namespace ApiSecurity.Service.Menu
{
    public class MenuServices : IMenuServices
    {
        public List<MenuEntities> GetMenuByRolId(int RolId)
        {
            try
            {
                return new ConnectionData().SqlQuery<MenuEntities>(@"""public"".""sp_CatMenuGetByRolId""", RolId);
            }
            catch (Exception ex)
            {
                //Controlar Errores aqui REGISTRAR EN BD
                throw new Exception(ex.Message);
            }
        }
    }
}

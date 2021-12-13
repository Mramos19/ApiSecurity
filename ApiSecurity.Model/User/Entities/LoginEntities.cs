using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSecurity.Model.User.Entities
{
    public class LoginEntities
    {
        public int UserId { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; }
    }
}

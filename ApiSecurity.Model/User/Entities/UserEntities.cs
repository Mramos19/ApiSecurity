using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSecurity.Model.User.Entities
{
    public class UserEntities
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }
        public string UserImageUrl { get; set; }
        public int UserRolId { get; set; }
        public bool UserStatus { get; set; }
    }
}

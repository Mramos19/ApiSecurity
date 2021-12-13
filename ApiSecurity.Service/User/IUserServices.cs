using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ApiSecurity.Model.User.Entities;
using ApiSecurity.Model.User.Response;
using ApiSecurity.Model.User.Request;

namespace ApiSecurity.Service.User
{
    public interface IUserServices
    {
        public List<LoginEntities> Login(LoginRequest Request);
        public List<UserEntities> GetUserById(int UserId);
    }
}

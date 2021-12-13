using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiSecurity.Model.User.Entities;

namespace ApiSecurity.Service.Token
{
    public interface ITokenServices
    {
        public string GenerateToken(LoginEntities User, string SecretKey, double DayExpired);

    }
}

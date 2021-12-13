using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ApiSecurity.Data;
using ApiSecurity.Model.User.Entities;
using ApiSecurity.Model.User.Request;
using ApiSecurity.Model.User.Response;

namespace ApiSecurity.Service.User
{
    public class UserServices : IUserServices
    {
        public List<LoginEntities> Login(LoginRequest Request)
        {
            try
            {
                var _Cnn = new ConnectionData();
                var _result = _Cnn.SqlQuery<LoginEntities>(@"""public"".""sp_CatUserGetByCredential""", Request.Email, Request.Password);
                return _result;
            }
            catch (Exception ex)
            {
                //Guardar el en tabla de Log de errores
                throw new Exception(ex.Message);
            }
        }

        public List<UserEntities> GetUserById(int UserId)
        {
            try
            {
                return new ConnectionData().SqlQuery<UserEntities>(@"""public"".""sp_CatUserGetById""", UserId);
            }
            catch (Exception ex)
            {
                //Guardar el en tabla de Log de errores
                throw new Exception(ex.Message);
            }
        }
    }
}

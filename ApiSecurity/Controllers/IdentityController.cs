using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

//Entities
using ApiSecurity.Entities;
using ApiSecurity.Model.User.Entities;
using ApiSecurity.Model.User.Request;
using ApiSecurity.Model.User.Response;
using ApiSecurity.Model.Menu.Entities;

//Services
using ApiSecurity.Service.Token;
using ApiSecurity.Service.User;
using ApiSecurity.Service.Menu;


namespace ApiSecurity.Controllers
{
    [Route("Authentication/Services")]
    [ApiController]
    public class IdentityController : ControllerBase
    {


        private ITokenServices _TokenService;
        private IUserServices _UserService;
        private IMenuServices _MenuService;


        public IdentityController(ITokenServices TokenService, IUserServices UserService, IMenuServices MenuService)
        {
            _TokenService = TokenService;
            _UserService = UserService;
            _MenuService = MenuService;
        }


        [HttpPost("Login")]
        public ResponseEntities<LoginResponse> Login(LoginRequest Request)
        {

            ResponseEntities<LoginResponse> _Response = new ResponseEntities<LoginResponse>();

            try
            {

                List<LoginEntities> _LoginEntities = _UserService.Login(Request);


                if (_LoginEntities.Count > 0)
                {
                    _Response.StatusCode = "00";
                    _Response.Message = "Success";                    

                    _Response.Result = new LoginResponse()
                    {
                        Token = _TokenService.GenerateToken(_LoginEntities.FirstOrDefault(), Utility.GetKey<string>("SecretKey"), Utility.GetKey<double>("Day"))
                    };
                }
                else
                {
                    _Response.StatusCode = "01";
                    _Response.Message = "Email o contraseña incorrectos";
                }         
            }
            catch (Exception ex)
            {
                _Response.StatusCode = "05";
                _Response.Message = ex.Message;
            }

            return _Response;

        }

        [HttpPost("ValidToken")]
        public ResponseEntities<bool> ValidToken()
        {


            ResponseEntities<bool> _Response = new ResponseEntities<bool>();

            try
            {
                //Validando que exista una autenticacion
                if (User.Identity.IsAuthenticated)
                {
                    var identity = User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        //Obtenemos los datos que estan encriptados en el token
                        IEnumerable<Claim> claims = identity.Claims;

                        //Validanto que el usuario exista y este activo en la Bd
                        int UserId = int.Parse(claims.FirstOrDefault(c => c.Type == "UserId").Value);
                        Int16 RolId = Int16.Parse(claims.FirstOrDefault(c => c.Type == "RolId").Value);

                        DateTime Expired = DateTime.Now;

                        if (HttpContext.Request.Headers.TryGetValue("Authorization", out var _Token))
                        {
                            JwtSecurityToken Token = new JwtSecurityToken(_Token.ToString().Replace("Bearer ", ""));
                            Expired = Convert.ToDateTime(Token.ValidTo);
                        }

                        //Validando Expiracion del token

                        if (DateTime.Now <= Expired)
                        {

                            //Validandar el usuario 
                            _Response.StatusCode = "00";
                            _Response.Message = "Success";
                            _Response.Result = true;

                        }
                        else
                        {
                            _Response.StatusCode = "04";
                            _Response.Message = "Token expired";
                            _Response.Result = false;
                        }

                    }
                    else
                    {
                        _Response.StatusCode = "05";
                        _Response.Message = "Unauthorized";
                        _Response.Result = false;
                    }
                }
                else
                {
                    _Response.StatusCode = "05";
                    _Response.Message = "Unauthorized";
                    _Response.Result = false;
                }

            }
            catch (Exception ex)
            {
                _Response.StatusCode = "03";
                _Response.Message = ex.Message;
            }


            return _Response;
        }

        [HttpPost("GetMenu")]
        public ResponseEntities<List<MenuEntities>> GetMenu()
        {

            ResponseEntities<List<MenuEntities>> _Response = new ResponseEntities<List<MenuEntities>>();

            _Response.Result = null;

            try
            {

                var validToken = ValidToken();   //validando que el usuario que esta registrado en el token este autenticado

                _Response.StatusCode = validToken.StatusCode;
                _Response.Message = validToken.Message;

                //Si el token es válido y no ha expirado
                if (validToken.StatusCode == "00")
                {

                    var identity = User.Identity as ClaimsIdentity;

                    //Obtenemos los datos que estan encriptados en el token
                    IEnumerable<Claim> claims = identity.Claims;

                    int RolId = int.Parse(claims.FirstOrDefault(c => c.Type == "RolId").Value);

                    _Response.StatusCode = "00";
                    _Response.Message = "Success";
                    _Response.Result = _MenuService.GetMenuByRolId(RolId);

                }
            }
            catch (Exception ex)
            {
                _Response.StatusCode = "03";
                _Response.Message = ex.Message;
            }

            return _Response;

        }

        [HttpPost("GetUser")]
        public ResponseEntities<UserEntities> GetUser()
        {

            ResponseEntities<UserEntities> _Response = new ResponseEntities<UserEntities>();            

            try
            {

                var validToken = ValidToken();   //validando que el usuario que esta registrado en el token este autenticado

                _Response.StatusCode = validToken.StatusCode;
                _Response.Message = validToken.Message;

                //Si el token es válido y no ha expirado
                if (validToken.StatusCode == "00")
                {

                    var identity = User.Identity as ClaimsIdentity;

                    //Obtenemos los datos que estan encriptados en el token
                    IEnumerable<Claim> claims = identity.Claims;

                    int UserId = int.Parse(claims.FirstOrDefault(c => c.Type == "UserId").Value);

                    List<UserEntities> _lisUser = new List<UserEntities>();
                    _lisUser = _UserService.GetUserById(UserId);

                    _Response.StatusCode = "00";
                    _Response.Message = "Success";
                    _Response.Result = _lisUser.ToList().FirstOrDefault();

                }
                else
                {
                    _Response.StatusCode = validToken.StatusCode;
                    _Response.Message = validToken.Message;                    
                }
            }
            catch (Exception ex)
            {
                _Response.StatusCode = "03";
                _Response.Message = ex.Message;
            }

            return _Response;

        }

    }
}
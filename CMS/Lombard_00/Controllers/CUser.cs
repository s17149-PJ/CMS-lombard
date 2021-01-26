using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;

namespace Lombard_00.Controllers
{
    [ApiController]
    public class CUser : ControllerBase
    {
        public class LocalLoginClass
        {
            public string Nick { get; set; }
            public string Password { get; set; }
        }//done
        [Route("api/user/login")]
        [HttpPost]
        public TokenUser Login(LocalLoginClass login)
        {
            //fix incoming data
            var nick = login.Nick.Trim();
            var pass = login.Password.Trim();

            IDb db = IDb.DbInstance;
            lock (db)
            {
                //find and veryfiy
                var usr = db.FindTUser(login.Nick);
                if (usr == null ||
                    usr.Password != login.Password)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new TokenUser()
                    {
                        Success = false,
                        Id = -1,
                        Nick = null,
                        Name = null,
                        Surname = null,
                        Roles = null,
                        Token = null
                    };
                }
                //tokens
                var token = GetNewToken();
                usr.Token = token;
                usr.ValidUnitl = DateTime.Now.AddMinutes(11);
                db.ModifyTUser(usr);
                //send response
                return new TokenUser()
                {
                    Success = true,
                    Id = usr.Id,
                    Nick = usr.Nick,
                    Name = usr.Name,
                    Surname = usr.Surname,
                    Roles = usr.Roles.Select(e=>new Tranzit.TokenRole(e)),
                    Token = token
                };
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/user/keepAlive")]
        [HttpPost]
        public TokenUser RenewToken(TokenUser token)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                //find and veryfiy

                if (!TokenUser.IsUsrStillValid(token.Id, token.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new TokenUser()
                    {
                        Success = false,
                        Id = -1,
                        Nick = null,
                        Name = null,
                        Surname = null,
                        Roles = null,
                        Token = null
                    };
                }

                var usr = db.FindTUser(token.Id);
                //tokens
                var newtoken = GetNewToken();
                usr.Token = newtoken;
                usr.ValidUnitl = DateTime.Now.AddMinutes(11);
                db.ModifyTUser(usr);
                //send response
                return new TokenUser()
                {
                    Success = true,
                    Id = usr.Id,
                    Nick = usr.Nick,
                    Name = usr.Name,
                    Surname = usr.Surname,
                    Roles = usr.Roles.Select(e=>new Tranzit.TokenRole(e)),
                    Token = newtoken
                };
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public class LocalRegisterClass : LocalLoginClass
        {
            public string Name { get; set; }
            public string Surname { get; set; }
        }//done
        [Route("api/user/register")]
        [HttpPost]
        public TokenUser Register(LocalRegisterClass register)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                var usr = new TUser()
                {
                    Nick = register.Nick,
                    Name = register.Name,
                    Surname = register.Surname,
                    Password = register.Password
                };
                var token = GetNewToken();
                usr.Token = token;
                usr.ValidUnitl = DateTime.Now.AddMinutes(11);

                var value = db.AddTUser(usr);

                if (value == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new TokenUser()
                    {
                        Success = false,
                        Id = -1,
                        Nick = null,
                        Name = null,
                        Surname = null,
                        Roles = null,
                        Token = null
                    };
                }// db MAY refuse to create user. for now db demands Nick to be unique.

                return new TokenUser()
                {
                    Success = true,
                    Id = value.Id,
                    Nick = value.Nick,
                    Name = value.Name,
                    Surname = value.Surname,
                    Roles = value.Roles.Select(e=>new TokenRole(e)),
                    Token = token
                };
            }
        }//done
        //?redo to obj?
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public class LocalEdiitClass
        {
            public TokenUser TokenUser { get; set; }
            public string NewPassword { get; set; }
        }//done
        [Route("api/user/edit")]
        [HttpPost]
        public bool Edit(LocalEdiitClass edit)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(edit.TokenUser.Id, edit.TokenUser.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                var usr = db.FindTUser(edit.TokenUser.Id);
                //update
                usr.Nick = edit.TokenUser.Nick;
                usr.Name = edit.TokenUser.Name;
                usr.Surname = edit.TokenUser.Surname;
                usr.Password = edit.NewPassword;

                if (!db.ModifyTUser(usr))
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return false;
                }
                return true;
            }
        }//done
        //?redo to obj?
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private string GetNewToken()
        {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 255)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }//done
    }//?done
}

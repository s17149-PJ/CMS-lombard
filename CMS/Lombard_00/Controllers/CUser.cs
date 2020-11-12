using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [ApiController]
    public class CUser : ControllerBase
    {
        public class ActionLogin {
            public bool Success { get; set; }
            public int Id { get; set; }
            public string Nick { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public IEnumerable<string> Roles { get; set; }
            public string Token { get; set; }
        }

        [Route("api/user/login")]
        [HttpPost]
        public ActionLogin Auth(string nick, string password)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Nick == nick && usr.Password == password);
            if (usr == null) {

                return new ActionLogin()
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
            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);
            db.ModifyTUser(usr, usr);

            return new ActionLogin()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role.Name,
                Token = token
            };
        }//done

        [Route("api/user/keepAlive")]
        [HttpPost]
        public ActionLogin RenewToken(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);
            if (IsUsrStillValid(usr))
            {

                return new ActionLogin()
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

            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);
            db.ModifyTUser(usr, usr);

            return new ActionLogin()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role.Name,
                Token = token
            };
        }//done

        [Route("api/user/register")]
        [HttpPost]
        public ActionLogin Register(string Nick, string Name, string Surname, string Password)
        {
            IDb db = IDb.DbInstance;
            var usr = new TUser()
            {
                Nick = Nick,
                Name = Name,
                Surname = Surname,
                Password = Password
            };
            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);

            if(db.AddTUser(usr))
            {

                return new ActionLogin()
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

            usr = db.TUsers.Find(usr => usr.Nick == Nick);
            db.AddTUserRole(new TUserRole()
            {
                User = usr,
                Role = db.TRoles[1]
            });// auto add user role

            return new ActionLogin()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role.Name,
                Token = token
            };
        }//done

        [Route("api/user/edit")]
        [HttpPost]
        public bool Edit(int Id, string Nick, string Name, string Surname, string Password, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            usr.Nick = Nick;
            usr.Name = Name;
            usr.Surname = Surname;
            usr.Password = Password;
            
            return db.ModifyTUser(usr, usr);
        }//done

        [Route("api/user/list")]
        [HttpGet]
        public List<ActionLogin> List()
        {
            return (from TUser in IDb.DbInstance.TUsers select 
                new ActionLogin()
                {
                    Success = false,
                    Id = TUser.Id,
                    Nick = TUser.Nick,
                    Name = TUser.Name,
                    Surname = TUser.Surname,
                    Roles = from asoc in IDb.DbInstance.TUserRoles where asoc.User == TUser select asoc.Role.Name,
                    Token = null
                }).ToList();
        }//done

        private string GetNewToken() {
            var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 255)
               .Select(token => token[random.Next(token.Length)]).ToArray());

            return resultToken.ToString();
        }//done
        private bool IsUsrStillValid(TUser usr) {
            if (usr == null)
            {

                return false;
            }
            if (DateTime.Compare(usr.ValidUnitl, DateTime.Now) > 0)
            {

                return false;
            }

            return true;
        }//done
    }
}

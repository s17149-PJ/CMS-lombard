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
        [Route("api/user/login")]
        [HttpPost]
        public TokenUser Auth(string nick, string password)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Nick == nick && usr.Password == password);
            if (usr == null) {

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
            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);
            db.ModifyTUser(usr, usr);

            return new TokenUser()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role,
                Token = token
            };
        }//done

        [Route("api/user/keepAlive")]
        [HttpPost]
        public TokenUser RenewToken(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);
            if (IsUsrStillValid(usr))
            {

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

            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);
            db.ModifyTUser(usr, usr);

            return new TokenUser()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role,
                Token = token
            };
        }//done

        [Route("api/user/register")]
        [HttpPost]
        public TokenUser Register(string Nick, string Name, string Surname, string Password)
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

            usr = db.TUsers.Find(usr => usr.Nick == Nick);
            db.AddTUserRole(new TUserRole()
            {
                User = usr,
                Role = db.TRoles[1]
            });// auto add user role

            return new TokenUser()
            {
                Success = true,
                Id = usr.Id,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role,
                Token = token
            };
        }//done
        //?redo to obj?

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
        //?redo to obj?

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

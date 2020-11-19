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
            if (usr == null)
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

        [Route("api/user/keepAlive")]
        [HttpPost]
        public TokenUser RenewToken(int id, string token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == id && usr.Token == token);
            if (TokenUser.IsUsrStillValid(usr))
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

            var newtoken = GetNewToken();
            usr.Token = newtoken;
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
                Token = newtoken
            };
        }//done

        [Route("api/user/register")]
        [HttpPost]
        public TokenUser Register(string nick, string name, string surname, string password)
        {
            IDb db = IDb.DbInstance;
            var usr = new TUser()
            {
                Nick = nick,
                Name = name,
                Surname = surname,
                Password = password
            };
            var token = GetNewToken();
            usr.Token = token;
            usr.ValidUnitl = DateTime.Now.AddMinutes(11);

            var value = db.AddTUser(usr);

            if (value == null)
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

            usr = db.TUsers.Find(usr => usr.Nick == nick);
            db.AddTUserRole(new TUserRole()
            {
                User = usr,
                Role = db.TRoles[1]
            });// auto add user role

            return new TokenUser()
            {
                Success = true,
                Id = value.Id,
                Nick = value.Nick,
                Name = value.Name,
                Surname = value.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == value select asoc.Role,
                Token = token
            };
        }//done
        //?redo to obj?

        [Route("api/user/edit")]
        [HttpPost]
        public bool Edit(TokenUser tokenUser, string password)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == tokenUser.Id && usr.Token == tokenUser.Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            usr.Nick = tokenUser.Nick;
            usr.Name = tokenUser.Name;
            usr.Surname = tokenUser.Surname;
            usr.Password = password;

            return db.ModifyTUser(usr, usr);
        }//done
        //?redo to obj?

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

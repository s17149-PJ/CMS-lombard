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
            public string Nick { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public IEnumerable<string> Roles { get; set; }
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
                    Nick = null,
                    Name = null,
                    Surname = null,
                    Roles = null
                };
            }

            return new ActionLogin()
            {
                Success = true,
                Nick = usr.Nick,
                Name = usr.Name,
                Surname = usr.Surname,
                Roles = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role.Name
            };
        }

        [Route("api/user/keepAlive")]
        [HttpPost]
        public ActionLogin RenewToken(string token)
        {
            return null;
        }

        [Route("api/user/register")]
        [HttpPost]
        public ActionLogin Register()
        {
            return null;
        }

        [Route("api/user/edit")]
        [HttpPost]
        public ActionLogin Edit()
        {
            return null;
        }
    }
}

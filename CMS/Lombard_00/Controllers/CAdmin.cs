using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Data.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CAdmin : ControllerBase
    {
        [Route("api/admin/editRoles")]
        [HttpPost]
        public bool Edit(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            return true;
        }

        private bool IsUsrStillValid(TUser usr)
        {
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

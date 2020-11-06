using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CItem : ControllerBase
    {
        [Route("api/item/add")]
        [HttpPost]
        public bool Add(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            db.CleanUp();//daily cleanup of old items

            return true;
        }
        [Route("api/item/edit")]
        [HttpPost]
        public bool Edit(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/delete")]
        [HttpPost]
        public bool Delete(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/browse")]
        [HttpPost]
        public bool Browse(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/bid")]
        [HttpPost]
        public bool Bid(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/comment")]
        [HttpPost]
        public bool Comment(int Id, string Token)
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

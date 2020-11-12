using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Controllers.Tranzit;
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
        public bool ItemAdd(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            db.CleanUp();//daily cleanup of old items



            return true;
        }
        [Route("api/item/edit")]
        [HttpPost]
        public bool ItemEdit(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/delete")]
        [HttpPost]
        public bool ItemDelete(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/item/list")]
        [HttpPost]
        public List<TokenItem> ItemList(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return (from item in db.TItems
                    select
                        new TokenItem(item)).ToList();
        }//done
    }
}

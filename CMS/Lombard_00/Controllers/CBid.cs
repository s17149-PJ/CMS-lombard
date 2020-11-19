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
    public class CBid : ControllerBase
    {
        [Route("api/bid/create")]
        [HttpPost]
        public bool BidCreate(int id, string token, TokenBid bid)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == id && usr.Token == token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            if (db.TryToFinishDeal(new TItem() { Id = bid.Item.Id }))
                return false;

            var value = db.AddTUserItemBid(new TUserItemBid()
            { 
                Item = new TItem() { Id = bid.Item.Id },
                User = new TUser() { Id = bid.User.Id },
                CreatedOn = DateTime.Now,
                Money = bid.Money
            });

            if (value == null)
                return false;

            return true;
        }//done
        [Route("api/bid/delete")]
        [HttpPost]
        public bool BidDelete(int id, string token, TokenBid bid)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == id && usr.Token == token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var toDel = db.TUserItemBids.Find(ite => ite.Id == bid.Id);

            if (toDel == null)
                return false;//must exist
            if (toDel.User.Id != usr.Id)
                return false;//must be owner

            if (toDel.Item.WinningBid == toDel || toDel.Item.StartingBid == toDel)
                return false;//can't delete connection bids

            return db.RemoveTUserItemBid(toDel);
        }//done
        [Route("api/bid/list")]
        [HttpPost]
        public List<TokenBid> BidList(int id, string token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == id && usr.Token == token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return (from bid in db.TUserItemBids select new TokenBid(bid)).ToList();
        }//todo add serach
    }
}

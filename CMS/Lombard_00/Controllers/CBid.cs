using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
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
        public bool BidCreate(int Id, string Token, TokenBid Bid)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/bid/delete")]
        [HttpPost]
        public bool BidDelete(int Id, string Token, TokenBid Bid)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var toDel = db.TUserItemBids.Find(ite => ite.Id == Bid.Id);

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
        public List<TokenBid> BidList(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return (from bid in db.TUserItemBids select new TokenBid(bid)).ToList();
        }//todo add serach
    }
}

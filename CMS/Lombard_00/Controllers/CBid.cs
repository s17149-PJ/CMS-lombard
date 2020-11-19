using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public class LocalBidClass {
            public TokenUser User { get; set; }
            public TokenBid Bid { get; set; }
        }
        [Route("api/bid/create")]
        [HttpPost]
        public TokenBid BidCreate(LocalBidClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //check if deal is already done
            if (db.TryToFinishDeal(new TItem() { Id = pack.Bid.Item.Id }))
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return null;
            }
            //try to add item
            var value = db.AddTUserItemBid(new TUserItemBid()
            { 
                Item = new TItem() { Id = pack.Bid.Item.Id },
                User = new TUser() { Id = pack.Bid.User.Id },
                CreatedOn = DateTime.Now,
                Money = pack.Bid.Money
            });
            //sucsess?
            if (value == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //yes
            return new TokenBid(value);
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/bid/delete")]
        [HttpPost]
        public bool BidDelete(LocalBidClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }

            var toDel = db.FindTUserItemBid(pack.Bid.Id);

            if (toDel == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must exist
            if (toDel.User.Id != usr.Id)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must be owner
            if (toDel.Item.WinningBid == toDel || toDel.Item.StartingBid == toDel)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//can't delete connection bids

            return db.RemoveTUserItemBid(toDel);
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/bid/list")]
        [HttpPost]
        public IEnumerable<TokenBid> BidList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(user.Id);
            if (TokenUser.IsUsrStillValid(usr, user.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            return db.TUserItemBids.Select(bid=>new TokenBid(bid));
        }//todo add serach
    }
}

using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Lombard_00.Controllers
{
    [ApiController]
    public class CBid : ControllerBase
    {
        public class LocalBidClass
        {
            //public TokenUser User { get; set; }
            //public TokenBid Bid { get; set; }
            public int UserId { get; set; }
            public string Token { get; set; }
            public int SubjectId { get; set; }
            public Decimal Money { get; set; }
            public bool IsRating { get; set; }
        }
        [Route("api/bid/create")]
        [HttpPost]
        public TokenBid BidCreate(LocalBidClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.UserId, pack.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }
                //check if deal is already done
                if (db.TryToFinishDeal(new TItem() { Id = pack.SubjectId }))
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return null;
                }
                //try to add item
                var value = db.AddTUserItemBid(new TUserItemBid()
                {
                    Item = new TItem() { Id = pack.SubjectId },
                    User = new TUser() { Id = pack.UserId },
                    CreatedOn = DateTime.Now,
                    Money = pack.Money,
                    IsRating = pack.IsRating
                });
                //sucsess?
                if (value == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }
                //yes
                return new TokenBid(value, db);
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/bid/delete")]
        [HttpPost]
        public bool BidDelete(LocalBidClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.UserId, pack.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                var usr = db.FindTUser(pack.UserId);
                var toDel = db.FindTUserItemBid(pack.SubjectId);

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

                if (!db.RemoveTUserItemBid(toDel,true))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }
                return true;
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/bid/list")]
        [HttpPost]
        public IEnumerable<TokenBid> BidList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(user.Id, user.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return db.TUserItemBids.Select(bid => new TokenBid(bid, db));
            }
        }//todo add serach
    }
}

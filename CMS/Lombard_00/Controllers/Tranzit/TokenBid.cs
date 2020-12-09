using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenBid
    {
        public TokenBid() { }
        public TokenBid(TUserItemBid bid, IDb context) 
        {
            Id = bid.Id;
            Item = new TokenItem(bid.Item, context);
            User = TokenUser.CallByToken(bid.User);
            CreatedOn = bid.CreatedOn;
            Money = bid.Money;

            //optional - rating
            IsRating = bid.IsRating;
        }//done
        public static TokenBid CallByTokenItem(TUserItemBid bid)
        {
            if (bid == null)
                return null;

            return new TokenBid()
            {
                Id = bid.Id,
                Item = null,//NO CIRCLES!
                User = TokenUser.CallByToken(bid.User),
                Money = bid.Money,
                CreatedOn = bid.CreatedOn
            };
        }//done

        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Money { get; set; }

        //optional - rating
        public bool IsRating { get; set; }
    }
}

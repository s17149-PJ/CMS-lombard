using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenBid
    {
        public TokenBid() { }
        public TokenBid(TUserItemBid bid, IDb context)
        {
            Id = bid.Id;
            Item = new TokenItem(bid.Item, context);
            User = TokenUser.CallByToken(bid.User, context);
            //CreatedOn = bid.CreatedOn;
            CreatedOn = bid.CreatedOn.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
            Money = bid.Money;

            //optional - rating
            IsRating = bid.IsRating;
        }//done
        public static TokenBid CallByTokenItem(TUserItemBid bid, IDb context)
        {
            if (bid == null)
                return null;

            return new TokenBid()
            {
                Id = bid.Id,
                Item = null,//NO CIRCLES!
                User = TokenUser.CallByToken(bid.User, context),
                Money = bid.Money,
                CreatedOn = bid.CreatedOn.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds
        };
        }//done

        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public Double CreatedOn { get; set; }
        public decimal Money { get; set; }

        //optional - rating
        public bool IsRating { get; set; }
    }
}

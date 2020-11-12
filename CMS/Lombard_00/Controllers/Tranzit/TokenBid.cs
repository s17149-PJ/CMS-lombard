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
        public static TokenBid CallByTokenItem(TUserItemBid bid)
        {
            return new TokenBid()
            {
                Id = bid.Id,
                Item = null,//NO CIRCLES!
                User = TokenUser.CallByTokenBid(bid.User),
                CreatedOn = bid.CreatedOn,
                Money = bid.Money
            };
        }//done

        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public SqlMoney Money { get; set; }
    }
}

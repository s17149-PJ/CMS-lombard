using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenBid
    {
        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public SqlMoney Money { get; set; }
    }
}

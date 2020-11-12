using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenComment
    {
        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public string Comment { get; set; }
    }
}

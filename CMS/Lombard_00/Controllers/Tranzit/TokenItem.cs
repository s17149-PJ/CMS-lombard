using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageMetaData { get; set; }
        public byte[] Image { get; set; }
        public TokenBid StartingBid { get; set; }
        public TokenBid WinningBid { get; set; }
    }
}

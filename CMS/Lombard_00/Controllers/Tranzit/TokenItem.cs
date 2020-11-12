using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenItem
    {
        public TokenItem() { }
        public TokenItem(TItem item) 
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            ImageMetaData = item.ImageMetaData;
            Image = item.Image;
            StartingBid = TokenBid.CallByTokenItem(item.StartingBid);
            WinningBid = TokenBid.CallByTokenItem(item.WinningBid);
        }//done

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageMetaData { get; set; }
        public byte[] Image { get; set; }
        public TokenBid StartingBid { get; set; }
        public TokenBid WinningBid { get; set; }
    }
}

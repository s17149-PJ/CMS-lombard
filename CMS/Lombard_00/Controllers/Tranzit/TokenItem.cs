using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenItem
    {
        public TokenItem() { }
        public TokenItem(TItem item, IDb context)
        {
            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
            ImageMetaData = item.ImageMetaData;
            Image = item.Image;
            StartingBid = TokenBid.CallByTokenItem(item.StartingBid, context);
            WinningBid = TokenBid.CallByTokenItem(item.WinningBid, context);
            FinallizationDateTime = item.FinallizationDateTime;
            FinallizationDateTimeDouble = (FinallizationDateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Tags = item.Tags.Select(e=>new TokenTag(e));

            var tbids = new List<TUserItemBid>(item.Bids);
            tbids.Remove(item.StartingBid);
            var tbid = tbids.OrderBy(e => e.Money).FirstOrDefault();
            if (tbid != null)
                WinningBid = TokenBid.CallByTokenItem(tbid, context);
            Bids = tbids.Select(e => TokenBid.CallByTokenItem(e, context));

            //optional - rating
            RatingAvarage = item.RatingAvarage;
            NumberOfRatings = item.NumberOfRatings;
        }//done

        public TokenSimpleItem Simplify()
        {
            return new TokenSimpleItem()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Image = ImageMetaData,
                FinallizationDateTime = FinallizationDateTime.ToString(),
                FinallizationDateTimeDouble = (new DateTime(1970, 1, 1) - FinallizationDateTime).TotalMilliseconds,
                Tags = Tags.Select(e => e.Name),
                RatingAvarage = RatingAvarage.ToString(),
                NumberOfRatings = NumberOfRatings.ToString()
            };
        }
        public TokenItem(TokenSimpleItem token)
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageMetaData { get; set; }
        public byte[] Image { get; set; }
        public TokenBid StartingBid { get; set; }//owner
        public TokenBid BestBid { get; set; }//current best
        public TokenBid WinningBid { get; set; }//winner <- after auction end
        public IEnumerable<TokenBid> Bids { get; set; }//all bids \ owner

        public DateTime FinallizationDateTime { get; set; }
        public double FinallizationDateTimeDouble { get; set; }
        //tag system
        public IEnumerable<TokenTag> Tags { get; set; }
        public string TagsString{ get; set; }
        //optional - rating
        public Decimal RatingAvarage { get; set; }
        public int NumberOfRatings { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    [Table("TItem")]
    public class TItem
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }

        public string ImageMetaData { get; set; }// format & size and stuff
        public byte[] Image { get; set; } // preview od product; other images (gallery will be in seprate class)

        public TUserItemBid StartingBid { get; set; }//also holds ref to prev owner
        public TUserItemBid WinningBid { get; set; }//holds ref to current owner
        [Column(TypeName = "datetime2")]
        public DateTime FinallizationDateTime { get; set; }

        //optional - rating
        public Decimal RatingAvarage { get; set; }
        public int NumberOfRatings { get; set; }
    }//done somethings. dunno how much do we want really
}

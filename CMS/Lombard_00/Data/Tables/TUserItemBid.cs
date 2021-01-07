﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    [Table("TBid")]
    public class TUserItemBid
    {
        [Key]
        public int Id { get; set; }
        public TItem Item { get; set; }
        public TUser User { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }
        public decimal Money { get; set; }

        //optional - rating
        public bool IsRating { get; set; }
    }//done I think. dunno.
}

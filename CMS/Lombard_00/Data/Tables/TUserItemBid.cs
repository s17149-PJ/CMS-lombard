using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    public class TUserItemBid
    {
        [Key]
        public int Id { get; set; }
        public TItem Item { get; set; }
        public TUser User { get; set; }

        public DateTime CreatedOn { get; set; }
        public decimal Money { get; set; }
    }//done I think. dunno.
}

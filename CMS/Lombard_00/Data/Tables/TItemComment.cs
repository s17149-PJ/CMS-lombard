using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    public class TItemComment
    {
        [Key]
        public int Id { get; set; }
        public TItem Item { get; set; }
        public TUser User { get; set; }
        [MaxLength(512)]
        public string Comment { get; set; }
    }//done
}

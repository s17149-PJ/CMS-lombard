using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    [Table("TItemTag")]
    public class TItemTag
    {
        [Key]
        public int Id { get; set; }
        public TTag Tag { get; set; }
        public TItem Item { get; set; }
    }
}

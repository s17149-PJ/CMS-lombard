using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    public class TNode
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime When { get; set; }
        public Decimal Value { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    public class TNode
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime When { get; set; }
        public Decimal ValueDecimal { get; set; }
        public string ValueString { get; set; }
    }
}

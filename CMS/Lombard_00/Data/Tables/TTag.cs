using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    [Table("TTag")]
    public class TTag
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<TItem> Items { get; set; }
    }
}

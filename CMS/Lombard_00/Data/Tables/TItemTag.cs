using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

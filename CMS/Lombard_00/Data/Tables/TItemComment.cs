using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    [Table("TComment")]
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

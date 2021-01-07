using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    [Table("TRole")]
    public class TRole
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        //optionally:
        //[MaxLength(255)]
        //public string Params { get; set; }

        public virtual ICollection<TUser> Users { get; set; }
    }//done
}

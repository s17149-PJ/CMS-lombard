using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lombard_00.Data.Tables
{
    [Table("TUserRole")]
    public class TUserRole
    {
        //stupid but works. 
        [Key]
        public int Id { get; set; }
        public TUser User { get; set; }
        public TRole Role { get; set; }
    }//done
}

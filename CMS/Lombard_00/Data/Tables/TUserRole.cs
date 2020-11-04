using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    public class TUserRole
    {
        [Key]
        public int Id { get; set; }
        public TUser User { get; set; }
        public TRole Role { get; set; }
    }
}

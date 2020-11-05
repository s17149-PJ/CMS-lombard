using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Tables
{
    [Table("TUser")]
    public class TUser
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nick { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
    }
}

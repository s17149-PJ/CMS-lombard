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
        //stupid but works. 
        [Key]
        public int Id { get; set; }
        public TUser User { get; set; }
        private TRole role;
        public TRole Role {
            get {
                if (role == null)
                    return new TRole() { Id = -1, Name = "error" };
                return role; }
            set { role = value; } 
        }
    }//done
}

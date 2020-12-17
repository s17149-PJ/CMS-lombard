﻿using System;
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
        //DON'T set Id. it should happen automatically. if no the it will be overwritten by EFDb anyway.
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
        // this is session part.
        [MaxLength(256)]
        public string Token { get; set; }
        //private DateTime validUnitl;
        [Column(TypeName = "datetime2")]
        public DateTime ValidUnitl { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime JoinedOn { get; set; }
        /*
         * get 
             {
                if (validUnitl == null)
                    return DateTime.Now.AddMinutes(-1);
                return validUnitl;
            } set {
                validUnitl = value;
            } }
        */
    }//done
}

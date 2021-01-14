using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class RoleToken
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleToken(TRole role) 
        {
            this.Id = role.Id;
            this.Name = role.Name;
        }
    }
}

using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers
{
    public class TokenUser
    {
        public bool Success { get; set; }
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<TRole> Roles { get; set; }
        public string Token { get; set; }

        public static bool IsUsrStillValid(TUser usr)
        {
            if (usr == null)
            {

                return false;
            }
            if (DateTime.Compare(usr.ValidUnitl, DateTime.Now) > 0)
            {

                return false;
            }

            return true;
        }//done
    }
}

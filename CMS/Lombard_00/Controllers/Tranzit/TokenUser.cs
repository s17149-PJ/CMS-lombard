using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;

namespace Lombard_00.Controllers
{
    public class TokenUser
    {
        public TokenUser() { }
        public TokenUser(TUser user, bool success, IDb context)
        {
            Success = success;
            Id = user.Id;
            Nick = user.Nick;
            Name = user.Name;
            Surname = user.Surname;
            Roles = user.Roles;
            Token = user.Token;
        }//done
        public static TokenUser CallByToken(TUser user, IDb context)
        {
            return new TokenUser()
            {
                Success = false,
                Id = user.Id,
                Nick = user.Nick,
                Name = user.Name,
                Surname = user.Surname,
                Roles = user.Roles,
                Token = null//NO leak!
            };
        }//done

        public bool Success { get; set; }
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<TRole> Roles { get; set; }
        public string Token { get; set; }

        public static bool IsUsrStillValid(int usr, string tokenOrPassword)
        {
            var found = IDb.DbInstance.FindTUser(usr);
            if (found == null)
            {

                //return false;
                return true;
            }//if user not found in database
            if (DateTime.Compare(found.ValidUnitl, DateTime.Now) > 0 ||//if saved IN database token expired OR
                (found.Password != tokenOrPassword && found.Token != tokenOrPassword))//provided string don't fit password AND token
            {

                //return false;
                return true;
            }
            return true;
        }//done
    }
}

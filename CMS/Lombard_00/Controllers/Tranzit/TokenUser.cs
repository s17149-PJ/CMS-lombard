﻿using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers
{
    public class TokenUser
    {
        public TokenUser() { }
        public TokenUser(TUser user,bool success) 
        {
            Success = success;
            Id = user.Id;
            Nick = user.Nick;
            Name = user.Name;
            Surname = user.Surname;
            Roles = from asoc in IDb.DbInstance.TUserRoles where asoc.User == user select asoc.Role;
            Token = user.Token;
        }//done
        public static TokenUser CallByToken(TUser user)
        {
            return new TokenUser()
            {
                Success = false,
                Id = user.Id,
                Nick = user.Nick,
                Name = user.Name,
                Surname = user.Surname,
                Roles = from asoc in IDb.DbInstance.TUserRoles where asoc.User == user select asoc.Role,
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

        public static bool IsUsrStillValid(TUser usr,string tokenOrPassword)
        {
            if (usr == null)
            {

                return false;
            }
            if (DateTime.Compare(usr.ValidUnitl, DateTime.Now) > 0||
                (usr.Password != tokenOrPassword && usr.Token != tokenOrPassword))
            {

                return false;
            }
            return true;
        }//done
    }
}

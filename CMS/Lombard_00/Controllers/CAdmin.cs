using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CAdmin : ControllerBase
    {
        [Route("api/admin/editRoles")]
        [HttpPost]
        public bool Edit(int Id, string Token, TokenUser NewSettings)
        {
            //check if logged in
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);
            if (IsUsrStillValid(usr))
                return false;
            //check if admin (role Id == 1)
            var rols = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role;
            if (!rols.Where(rol => rol.Id == 1).Any())
                return false;

            //actuall func
            usr = db.TUsers.Find(usr => usr.Id == NewSettings.Id);//user pending edit

            List<TUserRole> OldTUserRole = (from asoc in db.TUserRoles where asoc.User == usr select asoc).ToList();//current rolls

            //remove exessive rolles
            OldTUserRole
                    .Where(TUR =>
                        !(from item in NewSettings.Roles select item.Id)//select Id from new roles
                        .ToList()
                        .Contains(TUR.Role.Id))//select only TUR's that have Role.Id that is NOT in new settings
                    .ToList()
                    .ForEach(TUR => db.RemoveTUserRole(TUR));

            //add missing ones
            NewSettings.Roles
                .Where(ROL =>
                    !(from TUR in OldTUserRole select TUR.Role.Id)//select Id from old roles
                    .ToList()
                    .Contains(ROL.Id))//select only ROL's that have Role.Id that is NOT in OldTUserRole
                .ToList()
                .ForEach(ROL => db.AddTUserRole(new TUserRole() { User = usr, Role = ROL }));

            return true;
        }//done

        [Route("api/admin/users")]
        [HttpGet]
        public List<TokenUser> List(int Id, string Token)
        {
            //check if logged in
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);
            if (IsUsrStillValid(usr))
                return null;
            //check if admin (role Id == 1)
            var rols = from asoc in db.TUserRoles where asoc.User == usr select asoc.Role;
            if(!rols.Where(rol=>rol.Id==1).Any())
                return null;

            //actuall func
            return (from TUser in IDb.DbInstance.TUsers
                    select
                        new TokenUser()
                        {
                        Success = false,
                        Id = TUser.Id,
                        Nick = TUser.Nick,
                        Name = TUser.Name,
                        Surname = TUser.Surname,
                        Roles = from asoc in IDb.DbInstance.TUserRoles where asoc.User == TUser select asoc.Role,
                        Token = null
                        }).ToList();
        }//done

        private bool IsUsrStillValid(TUser usr)
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
    }//?done
}

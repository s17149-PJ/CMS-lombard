using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [ApiController]
    public class CAdmin : ControllerBase
    {
        public class AuthorizationUser
        {
            public bool Success { get; set; }
            public int Id { get; set; }
            public string Nick { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public IEnumerable<TRole> Roles { get; set; }
            public string Token { get; set; }
        }
    public class LocalEditClass {
            public TokenUser Admin { get; set; }
            public TokenUser Edited { get; set; }
        }//done
        [Route("api/admin/editRoles")]
        [HttpPost]
        public bool Edit(LocalEditClass pack)
        {
            //check if logged in
            IDb db = IDb.DbInstance;
            lock (db)
            {
                var usr = db.FindUser(pack.Admin.Id);
                if (!TokenUser.IsUsrStillValid(usr, pack.Admin.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                //check if admin (role Id == 1)
                var rols = db.FindTUserRoles(usr.Id).Select(e => e.Role);
                if (!rols.Where(rol => rol.Id == 1).Any())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                //find edited user
                usr = db.FindUser(pack.Edited.Id);
                //if record exists that is
                if (usr == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                List<TUserRole> OldTUserRole = db.FindTUserRoles(usr.Id);//current rolls

                //remove exessive rolles
                OldTUserRole
                        .Where(ToRemove =>
                            !pack.Edited.Roles
                            .Select(e => e.Id)//select Id from new roles
                            .ToList()
                            .Contains(ToRemove.Role.Id))//select only TUR's that have Role.Id that is NOT in new settings
                        .ToList()
                        .ForEach(ToRemove => db.RemoveTUserRole(ToRemove));

                //add missing ones
                pack.Edited.Roles
                    .Where(ToAdd =>
                        !OldTUserRole
                        .Select(e => e.Role.Id)//select Id from old roles
                        .ToList()
                        .Contains(ToAdd.Id))//select only ROL's that have Role.Id that is NOT in OldTUserRole
                    .ToList()
                    .ForEach(ToAdd => db.AddTUserRole(new TUserRole() { User = usr, Role = ToAdd }));

                return true;
            }
        }//done
         

        [Route("api/admin/users")]
        [HttpPost]
        public IEnumerable<TokenUser> List(AuthorizationUser admin)
        {
            //check if logged in
            IDb db = IDb.DbInstance;
            lock (db)
            {
                var usr = db.FindUser(admin.Id);
                if (!TokenUser.IsUsrStillValid(usr, admin.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                //check if admin (role Id == 1)
                var rols = db.FindTUserRoles(usr.Id).Select(e => e.Role);
                if (!rols.Where(rol => rol.Id == 1).Any())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                //actuall func
                return db.TUsers
                    .Select(e =>
                            new TokenUser()
                            {
                                Success = false,
                                Id = e.Id,
                                Nick = e.Nick,
                                Name = e.Name,
                                Surname = e.Surname,
                                Roles = db.FindTUserRoles(e.Id).Select(e => e.Role),
                                Token = null
                            });
            }
        }//done
    }//?done
}

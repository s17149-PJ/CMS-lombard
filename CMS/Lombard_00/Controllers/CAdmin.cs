using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
        public class LocalEditClass
        {
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
                if (!TokenUser.IsUsrStillValid(pack.Admin.Id, pack.Admin.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                var usr = db.FindTUser(pack.Admin.Id);
                if (!usr.Roles.Where(rol => rol.Id == 1).Any())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                //find edited user
                usr = db.FindTUser(pack.Edited.Id);
                //if record exists that is
                if (usr == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                usr.Roles.Except(pack.Edited.Roles).ToList().ForEach(e => db.RemoveTUserRole(usr, e));

                pack.Edited.Roles.Except(usr.Roles).ToList().ForEach(e => db.AddTUserRole(usr, e));

                return true;
            }
        }//done

        public class LocalUsers
        {
            public TokenUser Admin { get; set; }
            public int Limit { get; set; }
            public int Offset { get; set; }
        }//done
        [Route("api/admin/users")]
        [HttpPost]
        public IEnumerable<TokenUser> List(LocalUsers users)
        {
            //check if logged in
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(users.Admin.Id, users.Admin.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                var usr = db.FindTUser(users.Admin.Id);
                //check if admin (role Id == 1)
                if (!usr.Roles.Where(rol => rol.Id == 1).Any())
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
                                Roles = e.Roles,
                                Token = null
                            });
            }
        }//done


        public class AdminPanel
        {
            public decimal AllUsers { get; set; }
            public decimal CurrentlyOnline { get; set; }
            public decimal RecentNewUsers { get; set; }//last week

            public decimal AllItems { get; set; }
            public decimal ActiveItems { get; set; }//not yet sold
            public decimal CompletedItems { get; set; }//auction was won by someone
            public decimal TerminatedItems { get; set; }//aution not won by the time

            public decimal RecentMonyFlow { get; set; }//money earned from CompletedItems
            public decimal RecentLoginCount { get; set; }//login over last week
        }

        [Route("api/admin/stats")]
        [HttpPost]
        public AdminPanel GetStats(TokenUser admin)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {

                if (!TokenUser.IsUsrStillValid(admin.Id, admin.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                var usr = db.FindTUser(admin.Id);
                //check if admin (role Id == 1)
                if (!usr.Roles.Where(rol => rol.Id == 1).Any())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }


                return new AdminPanel()
                {
                    AllUsers = db.TUsers.Count(),
                    CurrentlyOnline = db.TUsers.Where(usr => DateTime.Compare(usr.ValidUnitl, DateTime.Now) < 0).Count(),
                    RecentNewUsers = db.TUsers.Where(usr => DateTime.Compare(usr.JoinedOn, DateTime.Now.AddDays(-7)) > 0).Count(),
                    AllItems = db.TItems.Count(),
                    ActiveItems = db.TItems.Where(ite => DateTime.Compare(ite.FinallizationDateTime, DateTime.Now) < 0).Count(),
                    CompletedItems = db.TItems.Where(ite => ite.WinningBid != null).Count(),
                    TerminatedItems = db.TItems.Where(ite =>
                        DateTime.Compare(ite.FinallizationDateTime, DateTime.Now) < 0 ||
                        ite.WinningBid == null).Count(),
                    RecentMonyFlow = db.TItems.Where(ite => ite.WinningBid != null).Select(ite => ite.WinningBid.Money).Sum(),
                    RecentLoginCount = db.Log.Where(log => log.Key == "ActionLogin" &&
                        DateTime.Compare(log.When, DateTime.Now.AddDays(-7)) > 0).Count()
                };
            }
        }
    }//?done
}

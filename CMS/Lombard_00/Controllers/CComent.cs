using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CComent : ControllerBase
    {
        [Route("api/comment/create")]
        [HttpPost]
        public bool CommentCreate(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/comment/delete")]
        [HttpPost]
        public bool CommentDelete(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/comment/edit")]
        [HttpPost]
        public bool CommentEdit(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return true;
        }
        [Route("api/comment/list")]
        [HttpPost]
        public List<TokenComment> CommentList(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return null;
        }
    }
}

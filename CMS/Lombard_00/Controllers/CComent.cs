using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lombard_00.Controllers.Tranzit;
using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CComent : ControllerBase
    {
        public class LocalCommentClass {
            public TokenUser User { get; set; }
            public TokenComment Comment { get; set; }
        }
        [Route("api/comment/create")]
        [HttpPost]
        public TokenComment CommentCreate(LocalCommentClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            return new TokenComment(db.AddTItemComment(new TItemComment() 
            { 
                Item = db.FindTItem(pack.Comment.Item.Id),
                User = db.FindUser(pack.Comment.User.Id),
                Comment = pack.Comment.Comment
            }));
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/comment/delete")]
        [HttpPost]
        public bool CommentDelete(LocalCommentClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }

            var toDel = db.FindTItemComment(pack.Comment.Id);

            if (toDel == null)
                return false;//must exist
            if (toDel.User.Id != usr.Id)
                return false;//must be owner

            if (!db.RemoveTItemComment(toDel)) {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }
            return true;
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/comment/edit")]
        [HttpPost]
        public bool CommentEdit(LocalCommentClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }

            var com = new TItemComment()
            {
                Id = pack.Comment.Id,
                Comment = pack.Comment.Comment
            };

            if (!db.ModifyTItemComment(com, com)) {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }
            return true;
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/comment/list")]
        [HttpPost]
        public IEnumerable<TokenComment> CommentList(LocalCommentClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            return db.TItemComments.Select(e=>new TokenComment(e));
        }//todo add serach
    }
}

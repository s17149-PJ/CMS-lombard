using System;
using System.Collections.Generic;
using System.Linq;
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
        [Route("api/comment/create")]
        [HttpPost]
        public bool CommentCreate(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            return db.AddTItemComment(new TItemComment() 
            { 
                Item = new TItem() { Id = Comment.Item.Id },
                User = new TUser() { Id = Comment.User.Id },
                Comment = Comment.Comment
            });
        }//done
        [Route("api/comment/delete")]
        [HttpPost]
        public bool CommentDelete(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var toDel = db.TItemComments.Find(ite => ite.Id == Comment.Id);

            if (toDel == null)
                return false;//must exist
            if (toDel.User.Id != usr.Id)
                return false;//must be owner

            return db.RemoveTItemComment(toDel);
        }//done
        [Route("api/comment/edit")]
        [HttpPost]
        public bool CommentEdit(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var com = new TItemComment()
            {
                Id = Comment.Id,
                Comment = Comment.Comment
            };

            return db.ModifyTItemComment(com,com);
        }//done
        [Route("api/comment/list")]
        [HttpPost]
        public List<TokenComment> CommentList(int Id, string Token, TokenComment Comment)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return (from comment in db.TItemComments select new TokenComment(comment)).ToList();
        }//todo add serach
    }
}

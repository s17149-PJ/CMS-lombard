﻿using Lombard_00.Data.Db;
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Controllers.Tranzit
{
    public class TokenComment
    {
        public TokenComment() { }
        public TokenComment(TItemComment comment, IDb context)
        {
            Id = comment.Id;
            Item = new TokenItem(comment.Item, context);
            User = TokenUser.CallByToken(comment.User, context);
            Comment = comment.Comment;
        }//done

        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public string Comment { get; set; }
    }
}

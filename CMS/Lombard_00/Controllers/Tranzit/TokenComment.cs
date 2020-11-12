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
        public TokenComment(TItemComment comment)
        {
            Id = comment.Id;
            Item = new TokenItem(comment.Item);
            User = TokenUser.CallByToken(comment.User);
            Comment = comment.Comment;
        }//done


        public static TokenComment CallByTokenItem(TItemComment comment) 
        {
            return null;
        }

        public int Id { get; set; }
        public TokenItem Item { get; set; }
        public TokenUser User { get; set; }
        public string Comment { get; set; }
    }
}

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
    public class CItem : ControllerBase
    {
        [Route("api/item/add")]
        [HttpPost]
        public bool ItemAdd(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            db.CleanUp();//daily cleanup of old items
            
            var addedItem = db.AddTItem(
                new TItem() { 
                    Name = Item.Name,
                    Description = Item.Description,
                    ImageMetaData = Item.ImageMetaData,
                    Image = Item.Image,
                });
            
            if (addedItem == null)
                return false;

            var addedBid = db.AddTUserItemBid(
                new TUserItemBid() { 
                    Item = addedItem,
                    User = usr,
                    CreatedOn = DateTime.Now,
                    Money = Item.StartingBid.Money
                });

            if (addedBid == null)
                return false;

            addedItem.StartingBid = addedBid;

            return db.ModifyTItem(addedItem,addedItem);
        }//dones
        [Route("api/item/delete")]
        [HttpPost]
        public bool ItemDelete(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var toDel = db.TItems.Find(ite => ite.Id == Item.Id);
            
            if (toDel == null)
                return false;//must exist
            if (toDel.StartingBid.User.Id != usr.Id)
                return false;//must be owner

            return db.RemoveTItem(toDel);
        }//done
        [Route("api/item/edit")]
        [HttpPost]
        public bool ItemEdit(int Id, string Token, TokenItem Item)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return false;

            var ite = new TItem()
            {
                Id = Item.Id,
                Name = Item.Name,
                Description = Item.Description,
                ImageMetaData = Item.ImageMetaData,
                Image = Item.Image
            };

            return db.ModifyTItem(ite,ite);
        }//done

        [Route("api/item/list")]
        [HttpPost]
        public List<TokenItem> ItemList(int Id, string Token)
        {
            IDb db = IDb.DbInstance;
            var usr = db.TUsers.Find(usr => usr.Id == Id && usr.Token == Token);

            if (TokenUser.IsUsrStillValid(usr))
                return null;

            return (from item in db.TItems select new TokenItem(item)).ToList();
        }//done
    }
}

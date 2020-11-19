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
    public class CItem : ControllerBase
    {
        public class LocalItemClass {
            public TokenUser User { get; set; }
            public TokenItem Item { get; set; }
        }
        [Route("api/item/add")]
        [HttpPost]
        public TokenItem ItemAdd(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //must have starting bid
            if (pack.Item.StartingBid == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //daily cleanup of old items
            db.CleanUp();
            //add item
            var itemToAdd = new TItem() { 
                    Name = pack.Item.Name,
                    Description = pack.Item.Description,
                    ImageMetaData = pack.Item.ImageMetaData,
                    Image = pack.Item.Image
            };
            //!?SHOULD be automatically added to db
            itemToAdd.StartingBid = 
                new TUserItemBid()
                {
                    Item = itemToAdd,
                    User = usr,
                    CreatedOn = DateTime.Now,
                    Money = pack.Item.StartingBid.Money
                };
            //add
            itemToAdd = db.AddTItem(itemToAdd);
            //sucsess?
            if (itemToAdd == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //yes
            return new TokenItem(itemToAdd);
        }//done
        [Route("api/item/delete")]
        [HttpPost]
        public bool ItemDelete(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
                return false;
            //find
            var toDel = db.FindTItem(pack.Item.Id);
            
            if (toDel == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must exist
            if (toDel
                .StartingBid
                .User
                .Id != usr.Id &&
                db
                .FindTUserRoles(usr.Id)
                .Select(e => e.Role)
                .Where(rol => rol.Id == 1)
                .Any())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must be owner or admin

            return db.RemoveTItem(toDel);
        }//done
        [Route("api/item/edit")]
        [HttpPost]
        public bool ItemEdit(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }
            //find
            var ite = db.FindTItem(pack.Item.Id);

            if (ite == null)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must exist
            if (ite
                .StartingBid
                .User
                .Id != usr.Id &&
                db
                .FindTUserRoles(usr.Id)
                .Select(e => e.Role)
                .Where(rol => rol.Id == 1)
                .Any())
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }//must be owner or admin
            //update
            if (pack.Item.Name != null)
                ite.Name = pack.Item.Name;
            if (pack.Item.Description != null)
                ite.Description = pack.Item.Description;
            if (pack.Item.ImageMetaData != null)
                ite.ImageMetaData = pack.Item.ImageMetaData;
            if (pack.Item.Image != null)
                ite.Image = pack.Item.Image;
            //return
            return db.ModifyTItem(ite,ite);
        }//done
        [Route("api/item/refresh")]
        [HttpPost]
        public TokenItem ItemRefreh(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(pack.User.Id);
            if (TokenUser.IsUsrStillValid(usr, pack.User.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            //return
            db.TryToFinishDeal(new TItem() { Id = pack.Item.Id });
            return new TokenItem(db.FindTItem(pack.Item.Id));
        }//done
        [Route("api/item/list")]
        [HttpPost]
        public List<TokenItem> ItemList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            var usr = db.FindUser(user.Id);
            if (TokenUser.IsUsrStillValid(usr, user.Token))
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            return (from item in db.TItems select new TokenItem(item)).ToList();
        }//done
    }
}

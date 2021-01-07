using Lombard_00.Controllers.Tranzit;
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
    public class CItem : ControllerBase
    {
        public class LocalItemClass
        {
            public TokenUser User { get; set; }
            public TokenItem Item { get; set; }
        }
        [Route("api/item/add")]
        [HttpPost]
        public TokenItem ItemAdd(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                var usr = db.FindTUser(pack.User.Id);
                //must have starting bid
                if (pack.Item.StartingBid == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }
                //daily cleanup of old items
                db.CleanUp();
                //add item
                var itemToAdd = new TItem()
                {
                    Name = pack.Item.Name,
                    Description = pack.Item.Description,
                    ImageMetaData = pack.Item.ImageMetaData,
                    Image = pack.Item.Image,
                    FinallizationDateTime = pack.Item.FinallizationDateTime
                };
                //!?SHOULD be automatically added to db
                itemToAdd.StartingBid =
                    new TUserItemBid()
                    {
                        Item = itemToAdd,
                        User = usr,
                        CreatedOn = DateTime.Now
                        //Money = pack.Item.StartingBid.Money
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
                return new TokenItem(itemToAdd, db);
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/item/delete")]
        [HttpPost]
        public bool ItemDelete(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                    return false;

                var usr = db.FindTUser(pack.User.Id);
                //find
                var toDel = db.FindTItem(pack.Item.Id);

                if (toDel == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }//must exist
                if (toDel
                    .StartingBid.User.Id != usr.Id &&
                    usr.Roles.Where(rol => rol.Id == 1).Any())
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }//must be owner or admin

                if (!db.RemoveTItem(toDel))
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return false;
                }
                return true;
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/item/edit")]
        [HttpPost]
        public bool ItemEdit(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }

                var usr = db.FindTUser(pack.User.Id);
                //find
                var ite = db.FindTItem(pack.Item.Id);

                if (ite == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }//must exist
                if (ite
                    .StartingBid.User.Id != usr.Id &&
                    usr.Roles.Where(rol => rol.Id == 1).Any())
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
                if (pack.Item.FinallizationDateTime != null)
                    ite.FinallizationDateTime = pack.Item.FinallizationDateTime;
                //return
                if (!db.ModifyTItem(ite, ite))
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return false;
                }
                return true;
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [Route("api/item/refresh")]
        [HttpPost]
        public TokenItem ItemRefreh(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }
                //return
                db.TryToFinishDeal(new TItem() { Id = pack.Item.Id });
                return new TokenItem(db.FindTItem(pack.Item.Id), db);
            }
        }//done
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------   
        [Route("api/item/list")]
        [HttpPost]
        public List<TokenItem> ItemList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(user.Id, user.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return (from item in db.TItems select new TokenItem(item, db)).ToList();
            }
        }//done

        public class LocalItemFindClass
        {
            public TokenUser User { get; set; }
            public List<String> Tags { get; set; }
        }
        [Route("api/item/find")]
        [HttpPost]
        public List<TokenItem> ItemFind(LocalItemFindClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return db
                    .FindTItems(
                        pack
                            .Tags
                            .Select(e => new TTag() { Id = -1, Name = e })
                            .ToList())
                    .Select(e => new TokenItem(e, db))
                    .ToList();
            }
        }//done

        [Route("api/item/tags")]
        [HttpPost]
        public List<TTag> TagList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (TokenUser.IsUsrStillValid(user.Id, user.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return db.TTags;
            }
        }//done

        //=========simple

        [Route("api/item/Slist")]
        [HttpPost]
        public List<SimpleTokenItem> SItemList()
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                return (from item in db.TItems select new TokenItem(item, db).Simplify()).ToList();
            }
        }//done
    }
}

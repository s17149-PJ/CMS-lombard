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
            public string Tags { get; set; }
            public Decimal value { get; set; }
        }
        [Route("api/item/add")]
        [HttpPost]
        public TokenItem ItemAdd(LocalItemClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                //daily cleanup of old items
                db.CleanUp();

                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                //add item
                var itemToAdd = new TItem()
                {
                    Name = pack.Item.Name,
                    Description = pack.Item.Description,
                    ImageMetaData = pack.Item.ImageMetaData,
                    Image = pack.Item.Image,
                    FinallizationDateTime = pack.Item.FinallizationDateTime,
                    Tags = pack.Tags.Split(((char)('c'))).Select(e => new TTag() { Name = e }).ToList()
                };
                //add
                itemToAdd = db.AddTItem(itemToAdd, new TUser() { Id = pack.User.Id },pack.value);
                
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

                if (!db.RemoveTItem(toDel,true))
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
                ite = new TItem() { Id = ite.Id };
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
                if (pack.Tags != null && pack.Tags != "")
                    ite.Tags = pack.Tags.Split(((char)('c'))).Select(e => new TTag() { Name = e }).ToList();
                //return
                if (!db.ModifyTItem(ite))
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
        public IEnumerable<TokenItem> ItemList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(user.Id, user.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return db.TItems.Select(e=>new TokenItem(e, db));
            }
        }//done

        public class LocalItemFindClass
        {
            public TokenUser User { get; set; }
            public List<string> Tags { get; set; }
            //public List<int> TagsId { get; set; }
            public int SortBy { get; set; }
        }
        public class FoundResult {
            public List<TokenItem> FoundItems { get; set; }
            public List<TokenTag> FoundTags { get; set; }
        }
        [Route("api/item/find")]
        [HttpPost]
        public FoundResult ItemFind(LocalItemFindClass pack)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (!TokenUser.IsUsrStillValid(pack.User.Id, pack.User.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                if (pack.Tags == null || pack.Tags.Count == 0)
                    return new FoundResult()
                    {
                        FoundItems = Sort(db.TItems.Select(e => new TokenItem(e, db)).ToList(),pack.SortBy),
                        FoundTags = new List<TokenTag>()
                    };
                    


                var TTags =
                        pack.Tags
                        .Select(e => new TTag() { Id = -1, Name = e })
                        .ToList();
                //TTags.AddRange(
                //        pack.TagsId
                //        .Select(e => new TTag() { Id = e })
                //        .ToList());
                var result = db.FindTItems(TTags);
                if (result == null)
                    return new FoundResult()
                    {
                        FoundItems = new List<TokenItem>(),
                        FoundTags = new List<TokenTag>()
                    };
                return new FoundResult()
                {
                    FoundItems = Sort(result.Items.Select(e => new TokenItem(e,db)).ToList(),pack.SortBy),
                    FoundTags = result.Tags.Select(e=>new TokenTag(e)).ToList()
                };
            }
        }//done

        private List<TokenItem> Sort(List<TokenItem> what, int how) {
            switch (how){
                case 1: return what.OrderBy(e => e.Name).ToList();
                case 2: return what.OrderByDescending(e => e.Name).ToList();
                case 3: return what.OrderBy(e => e.FinallizationDateTime).ToList();
                case 4: return what.OrderByDescending(e => e.FinallizationDateTime).ToList();
                case 5: return what.OrderBy(e => e.WinningBid.Money).ToList();
                case 6: return what
                        .Select(e => new { value = e, money = (e.WinningBid==null)?(e.StartingBid.Money):(e.WinningBid.Money) })
                        .OrderByDescending(e => e.money)
                        .Select(e=>e.value)
                        .ToList();
                default: return what;
            }

        }

        [Route("api/item/tags")]
        [HttpPost]
        public List<TokenTag> TagList(TokenUser user)
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                if (TokenUser.IsUsrStillValid(user.Id, user.Token))
                {
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return null;
                }

                return db.TTags.Select(e=>new TokenTag(e)).ToList();
            }
        }//done

        //=========simple

        [Route("api/item/Slist")]
        [HttpPost]
        public List<TokenSimpleItem> SItemList()
        {
            IDb db = IDb.DbInstance;
            lock (db)
            {
                return (from item in db.TItems select new TokenItem(item, db).Simplify()).ToList();
            }
        }//done
    }
}

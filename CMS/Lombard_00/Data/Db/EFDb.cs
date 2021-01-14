
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Lombard_00.Data.Db
{
    public class EFDb : DbContext, IDb
    {
        /*all of this is interface stuff*/
        public List<TUser> TUsers
        {
            get
            {
                return CTUsers
                    .Include(e => e.Roles)
                    .Include(e => e.Comments)
                    .Include(e => e.Bids)
                    .Include(e => e.BroughtItems)
                    .Include(e => e.TakenItems)
                    .ToList();
            }
        }
        public TUser AddTUser(TUser user)
        {
            if (user.Roles == null)
                user.Roles = new List<TRole>();
            if (user.Comments == null)
                user.Comments = new List<TItemComment>();
            if (user.Bids == null)
                user.Bids = new List<TUserItemBid>();
            if (user.BroughtItems == null)
                user.BroughtItems = new List<TItem>();
            if (user.TakenItems == null)
                user.TakenItems = new List<TItem>();


            if (user.Roles.Count == 0)
                user.Roles.Add(TRoles[1]);

            //nick must be unique
            if (CTUsers.Where(usr => usr.Nick == user.Nick).Any())
                return null;

            var value = CTUsers.Add(user);
            SaveChanges();

            return value;
        }//done
        public TUser FindTUser(int Id)
        {
             return CTUsers
                    .Include(e => e.Roles)
                    .Include(e => e.Comments)
                    .Include(e => e.Bids)
                    .Include(e => e.BroughtItems)
                    .Include(e => e.TakenItems)
                    .Where(e => e.Id == Id)
                    .FirstOrDefault();
        }//done
        public TUser FindTUser(string UniqueNick)
        {
            return CTUsers
                    .Include(e => e.Roles)
                    .Include(e => e.Comments)
                    .Include(e => e.Bids)
                    .Include(e => e.BroughtItems)
                    .Include(e => e.TakenItems)
                    .AsEnumerable()
                    .Where(user => user.Nick == UniqueNick)
                    .FirstOrDefault();
        }//done
        public bool ModifyTUser(TUser newData)
        {
            var value = CTUsers.Find(newData.Id);
            if (value == null)
            {

                return false;
            }
            if (newData.Nick != null) 
                value.Nick = newData.Nick;
            if (newData.Name != null) 
                value.Name = newData.Name;
            if (newData.Surname != null) 
                value.Surname = newData.Surname;
            if (newData.Password != null) 
                value.Password = newData.Password;
            if (newData.Token != null) 
                value.Token = newData.Token;
            if (newData.ValidUnitl != null) 
                value.ValidUnitl = newData.ValidUnitl;
            SaveChanges();

            return true;
        }//done
        public void PokeDbLogin()
        {
            InternalData.Add(new TNode()
            {
                Key = "ActionLogin",
                When = DateTime.Now,
                ValueDecimal = 0,
                ValueString = ""
            }); ;
            SaveChanges();
        }

        public bool AddTUserRole(TUser user, TRole role,bool saveAway)
        {
            //are incoming values nulls?
            if (user == null ||
                role == null)
                return false;
            //find and replace to avoid duplication
            var fuser = CTUsers.Include(e=>e.Roles).Where(e=>e.Id==user.Id).FirstOrDefault();
            var frole = CTRoles.Include(e => e.Users).Where(e=>e.Id==role.Id).FirstOrDefault();
            //are there duplicates?
            if (fuser.Roles.Contains(frole) ||
                frole.Users.Contains(fuser))
                return false;
            //add and save
            fuser.Roles.Add(frole);
            frole.Users.Add(fuser);
            if(saveAway)
                SaveChanges();
            //saved
            return true;
        }//done
        public bool RemoveTUserRole(TUser user, TRole role, bool saveAway)
        {
            //are incoming values nulls?
            if (user == null ||
                role == null)
                return false;
            //find and replace to avoid duplication
            var fuser = CTUsers.Include(e => e.Roles).Where(e => e.Id == user.Id).FirstOrDefault();
            var frole = CTRoles.Include(e => e.Users).Where(e => e.Id == role.Id).FirstOrDefault();
            if (!fuser.Roles.Contains(frole) ||
                !frole.Users.Contains(fuser))
                return false;// missing or constraint error.
            //remove and save
            fuser.Roles.Remove(frole);
            frole.Users.Remove(fuser);
            if (saveAway)
                SaveChanges();
            //saved
            return true;
        }//done

        public List<TRole> TRoles
        {
            get
            {
                return CTRoles.Include(e => e.Users).ToList();
            }
        }//done
        public bool AddTRole(TRole role)
        {
            //name must be unique
            if (CTRoles.Where(rol => rol.Name == role.Name).Any())
                return false;

            if (role.Users == null)
                role.Users = new List<TUser>();

            CTRoles.Add(role);
            SaveChanges();

            return true;
        }//done
        public TRole FindRole(int Id)
        {
            return CTRoles.Include(e => e.Users).Where(e => e.Id == Id).FirstOrDefault();
        }//done
        public bool ModifyTRole(TRole newData)
        {
            var value = CTRoles.FirstOrDefault(value => value.Id == newData.Id);
            if (value == null)
            {

                return false;
            }
            if (newData.Name != null) value.Name = newData.Name;
            SaveChanges();

            return true;
        }//done

        public List<TItem> TItems
        {
            get
            {
                return CTItems
                        .Include(e => e.StartingBid)
                        .Include(e => e.WinningBid)
                        .Include(e => e.Bids)
                        .Include(e => e.Tags)
                        .Include(e => e.Comments)
                        .ToList();
            }
        }//done
        public TItem AddTItem(TItem item, TUser owner, Decimal value)
        {
            if (item.Tags == null)
                item.Tags = new List<TTag>();
            item.Bids = new List<TUserItemBid>();
            item.Comments = new List<TItemComment>();

            if (owner == null)
                return null;

            item.StartingBid = null;
            item.WinningBid = null;

            var found = FindTUser(owner.Id);
            if (found == null)
                return null;
            var bid = new TUserItemBid() {
                Item = item,
                User = found,
                CreatedOn = DateTime.Now,
                Money = value,
                IsRating = false
            };

            var added = CTItems.Add(item);
            SaveChanges();

            item.Bids.Add(bid);
            found.Bids.Add(bid);
            found.BroughtItems.Add(item);
            item.StartingBid = bid;

            TUserItemBids.Add(bid);
            SaveChanges();
            return added;
        }//done
        public bool ModifyTItem(TItem newData)
        {
            var value = FindTItem(newData.Id);
            if (value == null)
                return false;

            if (newData.Name != null)
                value.Name = newData.Name;
            if (newData.Description != null)
                value.Description = newData.Description;
            if (newData.ImageMetaData != null)
                value.ImageMetaData = newData.ImageMetaData;
            if (newData.Image != null)
                value.Image = newData.Image;
            if (newData.StartingBid != null)
                value.StartingBid = newData.StartingBid;
            if (newData.WinningBid != null)
                value.WinningBid = newData.WinningBid;
            if (newData.FinallizationDateTime != null)
                value.FinallizationDateTime = newData.FinallizationDateTime;
            if (newData.Tags != null) {
                //validate tags with db
                newData.Tags = newData.Tags.Select(e => HardFindTag(e)).ToList();

                var tagsToRemove = value.Tags.Except(newData.Tags).ToList();
                var tagsToAdd = newData.Tags.Except(value.Tags).ToList();

                tagsToRemove.ForEach(e => RemoveItemTag(value, e,false));
                tagsToAdd.ForEach(e => AddItemTag(value, e,false));
            }

            SaveChanges();

            return true;
        }//done
        public bool RemoveTItem(TItem item, bool saveAway)
        {
            var found = FindTItem(item.Id);
            var tags = found.Tags.ToList();
            var itemComments = found.Comments.ToList();
            var userItemBids = found.Bids.ToList();

            tags
                .ForEach(e => RemoveItemTag(found,e,false));
            itemComments
                .ForEach(e => RemoveTItemComment(e,false));
            userItemBids
                .ForEach(e => RemoveTUserItemBid(e,false));

            CTItems.Remove(item);

            if(saveAway)
                SaveChanges();

            return true;
        }//done
        public TItem FindTItem(int Id)
        {
            return CTItems
                    .Include(e => e.StartingBid)
                    .Include(e => e.WinningBid)
                    .Include(e => e.Bids)
                    .Include(e => e.Tags)
                    .Where(e => e.Id == Id)
                    .Include(e => e.Comments)
                    .FirstOrDefault();
        }//done

        public List<TItem> FindTItems(List<TTag> tags)
        {
            //find valid tags at all cost.
            var foundTags = tags.Select(e => HardFindTag(e)).Where(e => e != null).ToList();
            var count = foundTags.Count;
            //if found nothing ret error
            if (foundTags.Count() == 0)
                return null;
            //now it works better.
            return CTItems
                .Include(e => e.StartingBid)
                .Include(e => e.WinningBid)
                .Include(e => e.Tags)
                //.Where(e => e.Tags.Intersect(foundTags).Count() == count)
                .Where(e => !foundTags.Except(e.Tags).Any())// *should* be automatically optimized and stop on first missing tag. faster.
                .ToList();
        }//done

        public bool TryToFinishDeal(TItem item)
        {
            //find said item
            var foundItem = FindTItem(item.Id);
            if (foundItem == null)
                return false;
            //is there winnner already?
            if (foundItem.WinningBid != null)
                return false;
            //is it time to find one?
            if (DateTime.Compare(foundItem.FinallizationDateTime, DateTime.Now) > 0)
                return false;

            //get neccesary data
            var bids = new List<TUserItemBid>(foundItem.Bids.ToList())
                .OrderBy(e=>e.Money)
                .ToList();

            bids.Remove(foundItem.StartingBid);

            //was there any valid bid?
            if (bids.Count <= 0)
                return false;

            //who won?
            var wbid = bids.ToArray()[bids.Count - 1];

            //must be higher than starting bid
            if (wbid.Money < foundItem.StartingBid.Money)
                return false;

            //hurrah
            foundItem.WinningBid = wbid;
            wbid.User.TakenItems.Add(foundItem);

            SaveChanges();

            return true;
        }//NOPE <=====================================================================

        public List<TItemComment> TItemComments
        {
            get
            {
                return CTItemComments.Include(e => e.Item).Include(e => e.User).ToList();
            }
        }//done
        public TItemComment AddTItemComment(TItemComment comment)
        {
            var usr = FindTUser(comment.User.Id);
            if (usr == null)
                return null;
            comment.User = usr;
            usr.Comments.Add(comment);
            var ite = FindTItem(comment.Item.Id);
            if (ite == null)
                return null;
            comment.Item = ite;
            ite.Comments.Add(comment);

            var value = CTItemComments.Add(comment);
            SaveChanges();

            return value;
        }//done
        public bool ModifyTItemComment(TItemComment newData)
        {
            var value = CTItemComments.FirstOrDefault(value => value.Id == newData.Id);

            if (value == null)
                return false;

            if (newData.Comment != null) value.Comment = newData.Comment;
            SaveChanges();

            return true;
        }//done
        public bool RemoveTItemComment(TItemComment comment, bool saveAway)
        {
            var usr = FindTUser(comment.User.Id);
            if (usr == null)
                return false;
            comment.User = usr;
            var ite = FindTItem(comment.Item.Id);
            if (ite == null)
                return false;
            comment.Item = ite;

            CTItemComments.Remove(comment);
            if(saveAway)
                SaveChanges();

            return true;
        }//done
        public TItemComment FindTItemComment(int Id)
        {
            return CTItemComments
                .Include(e => e.Item)
                .Include(e => e.User)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }//done

        public List<TUserItemBid> TUserItemBids
        {
            get
            {
                return CTUserItemBids
                    .Include(e => e.Item)
                    .Include(e => e.User)
                    .ToList();
            }
        }//done
        public TUserItemBid AddTUserItemBid(TUserItemBid bid)
        {
            var usr = FindTUser(bid.User.Id);
            if (usr == null)
                return null;
            bid.User = usr;
            usr.Bids.Add(bid);
            var ite = FindTItem(bid.Item.Id);
            if (ite == null)
                return null;
            bid.Item = ite;

            var value = CTUserItemBids.Add(bid);

            ite.Bids.Add(value);

            if (bid.IsRating)
            {
                bid.Item.RatingAvarage =
                    ((bid.Item.RatingAvarage * bid.Item.NumberOfRatings)//get original value
                    + bid.Money) //alter
                    / (bid.Item.NumberOfRatings + 1);//create new one
                bid.Item.NumberOfRatings = bid.Item.NumberOfRatings + 1;//update amout
            }

            SaveChanges();

            return value;
        }//done
        public bool RemoveTUserItemBid(TUserItemBid bid, bool saveAway)
        {
            var bidfound = FindTUserItemBid(bid.Id);
            var usr = FindTUser(bidfound.User.Id);
            var ite = FindTItem(bidfound.Item.Id);

            usr.Bids.Remove(bidfound);
            ite.Bids.Remove(bidfound);
            CTUserItemBids.Remove(bidfound);

            if (bidfound.IsRating)
            {
                if (bidfound.Item.NumberOfRatings != 1)
                {
                    bidfound.Item.RatingAvarage =
                        ((bidfound.Item.RatingAvarage * bidfound.Item.NumberOfRatings)//get original value
                        - bidfound.Money) //alter
                        / (bidfound.Item.NumberOfRatings - 1);//create new one
                    bidfound.Item.NumberOfRatings = bidfound.Item.NumberOfRatings - 1;//update amout
                }
                else
                {
                    bidfound.Item.RatingAvarage = 0;
                    bidfound.Item.NumberOfRatings = 0;
                }
            }
            if(saveAway)
                SaveChanges();

            return true;
        }//done
        public TUserItemBid FindTUserItemBid(int Id)
        {
            return CTUserItemBids
                .Include(e => e.Item)
                .Include(e => e.User)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }//done

        public List<TTag> TTags
        {
            get
            {
                return CTTag
                    .Include(e => e.Items)
                    .ToList();
            }
        }//done
        public TTag AddTag(TTag tag)
        {
            var value = CTTag.Where(e => e.Name == tag.Name).FirstOrDefault();
            if (value != null)
                return value;

            if (tag.Items == null)
                tag.Items = new List<TItem>();

            value = CTTag.Add(tag);
            SaveChanges();

            return value;
        }//done
        public bool SoftRemoveTag(TTag tag)
        {
            var foundTag = HardFindTag(tag);

            if (foundTag == null)
                return false;

            foundTag.Items.ToList().ForEach(e => e.Tags.Remove(foundTag));
            foundTag.Items.Clear();

            CTTag.Remove(foundTag);

            SaveChanges();

            return true;
        }//done
        public TTag FindTag(int Id)
        {
            return CTTag
                .Include(e => e.Items)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }//done
        public TTag HardFindTag(TTag tag)
        {
            var value = CTTag
                .Include(e => e.Items)
                .Where(e => e.Name == tag.Name || e.Id == tag.Id)
                .FirstOrDefault();

            if (value == null) 
            {
                value = AddTag(tag);
            }

            return value;
        }

        public bool AddItemTag(TItem item, TTag tag, bool saveAway)
        {
            //are incoming values nulls?
            if (item == null ||
                tag == null)
                return false;

            //find and replace to avoid duplication
            var fitem = FindTItem(item.Id);
            var ftag = FindTag(tag.Id);

            //are there duplicates?
            if (fitem.Tags.Contains(ftag) ||
                ftag.Items.Contains(fitem))
                return false;

            //add and save
            fitem.Tags.Add(ftag);
            ftag.Items.Add(fitem);

            if(saveAway)
                SaveChanges();
            //saved
            return true;
        }//done
        public bool RemoveItemTag(TItem item, TTag tag, bool saveAway)
        {
            //are incoming values nulls?
            if (item == null ||
                tag == null)
                return false;

            //find and replace to avoid duplication
            var fitem = FindTItem(item.Id);
            var ftag = FindTag(tag.Id);

            // missing or constraint error.
            if (!fitem.Tags.Contains(ftag) ||
                !ftag.Items.Contains(fitem))
                return false;

            //remove and save
            fitem.Tags.Remove(ftag);
            ftag.Items.Remove(fitem);
            SaveChanges();

            //saved
            return true;
        }//done

        //chk func ---------------------------------------------------------------------------------------------------
        private DateTime LastChek = DateTime.Now;//start class with default value now
        public void CleanUp()
        {

            if (DateTime.Compare(LastChek, DateTime.Now) > 0)
                return;

            //delay next check untill tomorow
            LastChek = DateTime.Now.AddDays(1);

            //try to finish deals
            CTItems.ForEachAsync(e => TryToFinishDeal(e));

            //keep list of items to remove
            List<TItem> toRemove = new List<TItem>();

            //dunno if deleting during iteration will break it so I don't
            TItems
                .Where(item => DateTime.Compare(item.WinningBid.CreatedOn.AddYears(1), DateTime.Now) < 0)
                .ToList()
                .ForEach(item => toRemove.Add(item));

            //now having all refs del each item
            toRemove.ForEach(item =>RemoveTItem(item,false));

            SaveChanges();
        }// this method SHOULD be async. done

        public List<TNode> Log { get { return InternalData.ToList(); } }

        public void UpdateDb() 
        { 
            SaveChanges(); 
        }
        /*end of interface stuff*/

        /*start of EF stuff*/
        //public EFDb() { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TUser> CTUsers { get; set; }
        public DbSet<TRole> CTRoles { get; set; }

        public DbSet<TItem> CTItems { get; set; }
        public DbSet<TItemComment> CTItemComments { get; set; }
        public DbSet<TUserItemBid> CTUserItemBids { get; set; }

        public DbSet<TTag> CTTag { get; set; }

        public DbSet<TNode> InternalData { get; set; }
        /*
         * ma poważne wątpliwości co do autonumeracji EF. co więcej sporo rzeczy wymaga JOIN po stronie kontrollerów.
         * można by to zoptymalizować. kiedyś. na razie jako POC wystarczy.
         * 
         * ==> dodać tranzakcję przy dziwniejszych operacjach (usuwanie TItem)
         */
    }
}

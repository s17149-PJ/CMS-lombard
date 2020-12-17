
using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Db
{
    public class EFDb : DbContext ,IDb
    {
        /*all of this is interface stuff*/
        public List<TUser> TUsers { 
            get {
                return CTUsers.ToList();
            } 
        }
        public TUser AddTUser(TUser user)
        {
            //nick must be unique
            if (CTUsers.Where(usr =>usr.Nick == user.Nick).Any())
                return null;

            var value = CTUsers.Add(user);
            SaveChanges();

            AddTUserRole(new TUserRole()
            {
                User = value,
                Role = TRoles[1]
            });// auto add user role

            return value;
        }//done
        public TUser FindUser(int Id) {
            return CTUsers.Find(Id);
        }//done
        public TUser FindUser(string UniqueNick) {
            return CTUsers.AsEnumerable().Where(user => user.Nick == UniqueNick).FirstOrDefault();
        }//done
        public bool ModifyTUser(TUser toBeModified, TUser newData)
        {
            var value = CTUsers.FirstOrDefault(value => value.Id == toBeModified.Id);
            if (value == null) {

                return false;
            }
            if (newData.Nick     != null)   value.Nick       = newData.Nick;
            if (newData.Name     != null)   value.Name       = newData.Name;
            if (newData.Surname  != null)   value.Surname    = newData.Surname;
            if (newData.Password != null)   value.Password   = newData.Password;
            if (newData.Token != null)      value.Token      = newData.Token;
            if (newData.ValidUnitl != null) value.ValidUnitl = newData.ValidUnitl;
            SaveChanges();

            return true;
        }//done
        public void PokeDbLogin() {
            InternalData.Add(new TNode()
            {
                Key = "ActionLogin",
                When = DateTime.Now,
                Value = 0
            }); ;
            SaveChanges();
        }

        public List<TUserRole> TUserRoles
        {
            get
            {
                return CTUserRoles.Include(e=>e.Role).Include(e=>e.User).ToList();
            }
        }//done
        public bool AddTUserRole(TUserRole asoc)
        {
            //are incoming values nulls?
            if (asoc.User == null ||
                asoc.Role == null)
                return false;
            //find and replace to avoid duplication
            var tuserRole = new TUserRole() { User = FindUser(asoc.User.Id), Role = FindRole(asoc.Role.Id) };
            //are values found?
            if (tuserRole.User == null ||
                tuserRole.Role == null)
                return false;
            //are there duplicates?
            if (CTUserRoles
                .Include(e => e.Role)
                .Include(e => e.User)
                .AsEnumerable()
                .Where(e=> e.Role==asoc.Role && asoc.User == asoc.User)
                .Any())
                return false;
            //add and save
            CTUserRoles.Add(asoc);
            SaveChanges();
            //saved
            return true;
        }//done
        public List<TUserRole> FindTUserRoles(int UserId)
        {
            var User = FindUser(UserId);
            if (User == null)
                return new List<TUserRole>();
            return CTUserRoles
                .Include(e => e.Role)
                .Include(e => e.User)
                .AsEnumerable()
                .Where(e => e.User == User)
                .ToList();
        }//done
        public bool RemoveTUserRole(TUserRole asoc)
        {
            CTUserRoles.Remove(asoc);
            SaveChanges();

            return true;
        }//done (i think, haven't found any outer refs)

        public List<TRole> TRoles
        {
            get
            {
                return CTRoles.ToList();
            }
        }//done
        public bool AddTRole(TRole role)
        {
            //name must be unique
            if (CTRoles.Where(rol => rol.Name == role.Name).Any())
                return false;

            CTRoles.Add(role);
            SaveChanges();

            return true;
        }//done
        public TRole FindRole(int Id) {
            return CTRoles.Find(Id);
        }//done
        public bool ModifyTRole(TRole toBeModified, TRole newData)
        {
            var value = CTRoles.FirstOrDefault(value => value.Id == toBeModified.Id);
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
                return CTItems.Include(e=>e.StartingBid).Include(e=>e.WinningBid).ToList();
            }
        }//done
        public TItem AddTItem(TItem item)
        {
            var value = CTItems.Add(item);
            SaveChanges();

            return value;
        }//done
        public bool ModifyTItem(TItem toBeModified, TItem newData)
        {
            var value = CTItems.FirstOrDefault(value => value.Id == toBeModified.Id);
            if (value == null)
                return false;

            if (newData.Name          != null) 
                value.Name          = newData.Name;
            if (newData.Description   != null) 
                value.Description   = newData.Description;
            if (newData.ImageMetaData != null) 
                value.ImageMetaData = newData.ImageMetaData;
            if (newData.Image         != null) 
                value.Image         = newData.Image;
            if (newData.StartingBid   != null) 
                value.StartingBid   = newData.StartingBid;
            if (newData.WinningBid    != null) 
                value.WinningBid    = newData.WinningBid;
            if (newData.FinallizationDateTime != null)
                value.FinallizationDateTime = newData.FinallizationDateTime;

            SaveChanges();

            return true;
        }//done
        public bool RemoveTItem(TItem item)
        {
            List<TItemComment> itemComments = TItemComments
                .Where(comment => comment.Item == item)
                .ToList();
            List<TUserItemBid> userItemBids = TUserItemBids
                .Where(bid => bid.Item == item)
                .ToList();

            itemComments
                .ForEach(comment => RemoveTItem(item));
            userItemBids
                .ForEach(bid => RemoveTUserItemBid(bid));
            CTItems.Remove(item);
            SaveChanges();

            return true;
        }//done
        public TItem FindTItem(int Id) {
            return CTItems.Include(e => e.StartingBid).Include(e => e.WinningBid).Where(e=>e.Id==Id).FirstOrDefault();
        }//done

        /*IMPORTANT: this function have a nice O(n^2) cost so do NOT abuse it
         * at best use it to with few tags or uncommon tags. otherwise the serwer will die.
         */
        public List<TItem> FindTItems(List<TTag> tags) {
            //find valid tags at all cost.
            var foundTags = tags.Select(e => HardFindTag(e)).Where(e=>e!=null).ToList();
            //if found nothing ret error
            if (foundTags.Count() == 0)
                return null;

            var foundItems = CTItemTag
                .Include(e => e.Tag)
                .Include(e => e.Item)
                .Where(e => e.Tag == foundTags.ElementAt(0))
                .Select(e => e.Item);//those are initially found items. now on to filtering them out

            foundTags.RemoveAt(0);//remove already found tag
            
            var task = Task.Run(() => {
                if (foundTags.Count() != 0)//if there are still tags to process
                    foundItems =
                        foundItems.Where(item =>//for each item
                            /*(to self) all right here is the idea:
                             * we got this fancy list of items that HAVE ONE required tag.
                             * now we got to get rid of those that DO NOT have TItemTag with EVERY other tag that is on
                             * list foundTags. we do it in a following way:
                             * 
                             * we ask found tags if it has ANY tag that:
                             * DOES NOT have coressponding TItemTag that:
                             * -> HAVE TItem the same as looked up item
                             * -> HAVE TTag the sane as looked up tag
                             */
                            foundTags//get required tags
                                .Where(tag => // and find those

                                       !CTItemTag//that DO NOT have coresponding TItemTag 
                                           .Include(e => e.Tag)
                                           .Include(e => e.Item)
                                           .Where(e//that HAVE this item AND this tag
                                               => (e.Item == item
                                                && e.Tag == tag))
                                           .Any())//we ask if ANY of CTItemTag does meet those conditions NOT
                                                  //eg. if ALL of those NOT meet those conditions

                                .Any()//ANY that does NOT have proper TItemTag.
                        );//what remains are items that EACH have ALL of req tags.

                /*i have no slightest idea what I have just wrote. 
                 *it will go plaid agaist the wall the moment you will try to run it, probably
                 */
            });

            //kill if it lag serwer for more than: x
            if (task.Wait(TimeSpan.FromSeconds(5)))
                return foundItems.ToList();//return list
            else
                return foundItems.ToList();//return list
            //also i copypasted it (task idea) from stack so god know what it actually does
        }//dunno? mabe? mabe not? who knows. <=======================

        public bool TryToFinishDeal(TItem item) 
        {
            //find said item
            var foundItem = FindTItem(item.Id);
            //is there winnner already?
            if (foundItem.WinningBid != null)
                return false;
            //is it time to find one?
            if (DateTime.Compare(foundItem.FinallizationDateTime, DateTime.Now) > 0)
                return false;

            //get neccesary data
            var bids = CTUserItemBids
                .Include(e => e.Item)
                .Include(e => e.User)
                .Where(e => e.Item == foundItem)
                .OrderBy(e => e.Money)
                .ToList();
            bids.Remove(item.StartingBid);
            //was there any valid bid?
            if (bids.Count <= 0)
                return false;
            //who won?
            var wbid = bids.ToArray()[bids.Count - 1];
            //hurrah
            foundItem.WinningBid = wbid;
            SaveChanges();

            return true;
        }//NOPE <=======================

        public List<TItemComment> TItemComments
        {
            get
            {
                return CTItemComments.Include(e => e.Item).Include(e => e.User).ToList();
            }
        }//done
        public TItemComment AddTItemComment(TItemComment comment)
        {
            var usr = FindUser(comment.User.Id);
            if (usr == null)
                return null;
            comment.User = usr;
            var ite = FindTItem(comment.Item.Id);
            if (ite == null)
                return null;
            comment.Item = ite;

            var value = CTItemComments.Add(comment);
            SaveChanges();

            return value;
        }//done
        public bool ModifyTItemComment(TItemComment toBeModified, TItemComment newData)
        {
            var value = CTItemComments.FirstOrDefault(value => value.Id == toBeModified.Id);

            if (value == null)
                return false;

            if (newData.Comment != null) value.Comment = newData.Comment;
            SaveChanges();

            return true;
        }//done
        public bool RemoveTItemComment(TItemComment comment)
        {
            var usr = FindUser(comment.User.Id);
            if (usr == null)
                return false;
            comment.User = usr;
            var ite = FindTItem(comment.Item.Id);
            if (ite == null)
                return false;
            comment.Item = ite;

            CTItemComments.Remove(comment);
            SaveChanges();

            return true;
        }//done
        public TItemComment FindTItemComment(int Id) {
            return CTItemComments.Include(e => e.Item).Include(e => e.User).Where(e => e.Id == Id).FirstOrDefault();
        }//done

        public List<TUserItemBid> TUserItemBids
        {
            get
            {
                return CTUserItemBids.Include(e => e.Item).Include(e => e.User).ToList();
            }
        }//done
        public TUserItemBid AddTUserItemBid(TUserItemBid bid) 
        {
            var usr = FindUser(bid.User.Id);
            if (usr == null)
                return null;
            bid.User = usr;
            var ite = FindTItem(bid.Item.Id);
            if (ite == null)
                return null;
            bid.Item = ite;

            var value = CTUserItemBids.Add(bid);

            if (bid.IsRating) {
                bid.Item.RatingAvarage =
                    ((bid.Item.RatingAvarage * bid.Item.NumberOfRatings)//get original value
                    + bid.Money) //alter
                    / (bid.Item.NumberOfRatings + 1);//create new one
                bid.Item.NumberOfRatings = bid.Item.NumberOfRatings + 1;//update amout
            }

            SaveChanges();

            return value;
        }//done
        public bool RemoveTUserItemBid(TUserItemBid bid) 
        {
            var bidfound = FindTUserItemBid(bid.Id);
            var usr = FindUser(bidfound.User.Id);
            if (usr == null)
                return false;
            bidfound.User = usr;
            var ite = FindTItem(bidfound.Item.Id);
            if (ite == null)
                return false;
            bidfound.Item = ite;

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

            SaveChanges();

            return true;
        }//done
        public TUserItemBid FindTUserItemBid(int Id) {
            return CTUserItemBids.Include(e => e.Item).Include(e => e.User).Where(e => e.Id == Id).FirstOrDefault();
        }//done

        public List<TTag> TTags {
            get 
            {
                return CTTag.ToList();
            } 
        }//done
        public TTag AddTag(TTag tag) 
        {
            var value = CTTag.Where(e => e.Name == tag.Name).FirstOrDefault();
            if (value != null)
                return value;

            value = CTTag.Add(tag);
            SaveChanges();

            return value;
        }//done
        public bool SoftRemoveTag(TTag tag) 
        {
            var foundTag = FindTag(tag.Id);

            if (foundTag == null)
                foundTag = CTTag.Where(e=>e.Name==tag.Name).FirstOrDefault();

            if (foundTag == null)
                return false;

            if (CTItemTag.Include(e => e.Tag).Where(e => e.Tag == foundTag).Any())
                return false;

            CTTag.Remove(foundTag);
            SaveChanges();

            return true;
        }//done
        public bool HardRemoveTag(TTag tag)
        {
            var foundTag = FindTag(tag.Id);

            if (foundTag == null)
                foundTag = CTTag.Where(e => e.Name == tag.Name).FirstOrDefault();

            if (foundTag == null)
                return false;

            var toDel = CTItemTag.Include(e => e.Tag).Where(e => e.Tag == foundTag).ToList();
            toDel.ForEach(e => RemoveItemTag(e));

            CTTag.Remove(foundTag);
            SaveChanges();

            return true;
        }//done
        public TTag FindTag(int Id) {
            return CTTag.Find(Id);
        }//done
        public TTag HardFindTag(TTag tag) {
            var value = FindTag(tag.Id);
            if (value == null)
                value = CTTag.Where(e => e.Name == tag.Name).FirstOrDefault();

            return value;
        }
        public List<TTag> FindTags(TItem item) 
        {
            var found = FindTItem(item.Id);
            return CTItemTag
                    .Include(e => e.Item)
                    .Include(e => e.Tag)
                    .ToList()
                    .Where(e => e.Item == found)
                    .AsEnumerable()
                    .Select(e => e.Tag)
                    .ToList();
        }//done

        public List<TItemTag> TItemsTags { 
            get 
            {
                return CTItemTag
                    .Include(e => e.Item)
                    .Include(e => e.Tag)
                    .ToList();
            } 
        }//done
        public TItemTag AddItemTag(TItemTag itemTag)
        {
            var foundTag = FindTag(itemTag.Tag.Id);
            var foundItem = FindTItem(itemTag.Item.Id);

            if (foundTag == null)
                foundTag = AddTag(itemTag.Tag);

            if (CTItemTag
                .Include(e => e.Item)
                .Include(e => e.Tag)
                .Where(e => e.Item == foundItem)
                .Where(e => e.Tag == foundTag)
                .Any())
                return null;


            var value = CTItemTag.Add(
                new TItemTag()
                {
                    Item = foundItem,
                    Tag = foundTag
                });
            SaveChanges();

            return value;
        }//done
        public bool RemoveItemTag(TItemTag itemTag) 
        {
            var toDel = FindItemTag(itemTag.Id);
            if (toDel == null)
                return false;

            CTItemTag.Remove(toDel);

            return true;
        }//done
        public TItemTag FindItemTag(int Id)
        {
            return CTItemTag
                .Include(e => e.Item)
                .Include(e => e.Tag)
                .Where(e => e.Id == Id)
                .FirstOrDefault();
        }//done


        //chk func ---------------------------------------------------------------------------------------------------
        private DateTime LastChek = DateTime.Now;//start class with default value now
        public void CleanUp() 
        {
            if (DateTime.Compare(LastChek, DateTime.Now) > 0)
                return;
            //delay next check untill tomorow
            LastChek = DateTime.Now.AddDays(1);
            //keep list of items to remove
            List<TItem> toRemove = new List<TItem>();
            //async serach for items that are to  be removed
            //dunno if deleting during iteration will break it so I don't
            CTItems
                .Where(item => item.WinningBid != null)
                .ToList()
                .ForEach(item=> 
                {
                    if (DateTime.Compare(item.WinningBid.CreatedOn.AddYears(1), DateTime.Now) < 0)
                        toRemove.Add(item);
                });
            //now having all refs del each item
            toRemove.ForEach(item => RemoveTItem(item));
        }// this method SHOULD be async. done
        public void VoidOut() 
        {
            CTItems.RemoveRange(CTItems);

            CTUserRoles.RemoveRange(CTUserRoles);
            CTUsers.RemoveRange(CTUsers);
            CTRoles.RemoveRange(CTRoles);

            SaveChanges();
        }

        public List<TNode> Log { get { return InternalData.ToList(); } }
        /*end of interface stuff*/

        /*start of EF stuff*/
        //public EFDb() { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TUser> CTUsers { get; set; }
        public DbSet<TUserRole> CTUserRoles { get; set; }
        public DbSet<TRole> CTRoles { get; set; }

        public DbSet<TItem> CTItems { get; set; }
        public DbSet<TItemComment> CTItemComments { get; set; }
        public DbSet<TUserItemBid> CTUserItemBids { get; set; }

        public DbSet<TTag> CTTag { get; set; }
        public DbSet<TItemTag> CTItemTag { get; set; }

        public DbSet<TNode> InternalData { get; set; }
        /*
         * ma poważne wątpliwości co do autonumeracji EF. co więcej sporo rzeczy wymaga JOIN po stronie kontrollerów.
         * można by to zoptymalizować. kiedyś. na razie jako POC wystarczy.
         * 
         * ==> dodać tranzakcję przy dziwniejszych operacjach (usuwanie TItem)
         */
    }
}

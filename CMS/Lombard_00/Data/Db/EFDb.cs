﻿
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
            return CTUsers.AsQueryable().Where(user => user.Nick == UniqueNick).FirstOrDefault();
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

        public List<TUserRole> TUserRoles
        {
            get
            {
                return CTUserRoles.Include(e=>e.Role).Include(e=>e.User).ToList();
            }
        }
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
                .AsQueryable()
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
                .AsQueryable()
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
        }
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
        }
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
        public bool TryToFinishDeal(TItem item) 
        {

            return false;
        }

        public List<TItemComment> TItemComments
        {
            get
            {
                return CTItemComments.Include(e => e.Item).Include(e => e.User).ToList();
            }
        }
        public bool AddTItemComment(TItemComment comment)
        {
            var usr = TUsers.Where(e => e.Id == comment.User.Id).FirstOrDefault();
            if (usr == null)
                return false;
            comment.User = usr;
            var ite = TItems.Where(e => e.Id == comment.Item.Id).FirstOrDefault();
            if (ite == null)
                return false;
            comment.Item = ite;

            CTItemComments.Add(comment);
            SaveChanges();

            return true;
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
            var usr = TUsers.Where(e => e.Id == comment.User.Id).FirstOrDefault();
            if (usr == null)
                return false;
            comment.User = usr;
            var ite = TItems.Where(e => e.Id == comment.Item.Id).FirstOrDefault();
            if (ite == null)
                return false;
            comment.Item = ite;

            CTItemComments.Remove(comment);
            SaveChanges();

            return true;
        }//done

        public List<TUserItemBid> TUserItemBids
        {
            get
            {
                return CTUserItemBids.Include(e => e.Item).Include(e => e.User).ToList();
            }
        }
        public TUserItemBid AddTUserItemBid(TUserItemBid bid) 
        {
            var usr = TUsers.Where(e => e.Id == bid.User.Id).FirstOrDefault();
            if (usr == null)
                return null;
            bid.User = usr;
            var ite = TItems.Where(e => e.Id == bid.Item.Id).FirstOrDefault();
            if (ite == null)
                return null;
            bid.Item = ite;

            var value = CTUserItemBids.Add(bid);
            SaveChanges();

            return value;
        }//done
        public bool RemoveTUserItemBid(TUserItemBid bid) 
        {
            var usr = TUsers.Where(e => e.Id == bid.User.Id).FirstOrDefault();
            if (usr == null)
                return false;
            bid.User = usr;
            var ite = TItems.Where(e => e.Id == bid.Item.Id).FirstOrDefault();
            if (ite == null)
                return false;
            bid.Item = ite;

            CTUserItemBids.Remove(bid);
            SaveChanges();

            return true;
        }//done

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
        /*end of interface stuff*/

        /*start of EF stuff*/
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

        /*
         * ma poważne wątpliwości co do autonumeracji EF. co więcej sporo rzeczy wymaga JOIN po stronie kontrollerów.
         * można by to zoptymalizować. kiedyś. na razie jako POC wystarczy.
         * 
         * ==> dodać tranzakcję przy dziwniejszych operacjach (usuwanie TItem)
         */
    }
}


using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Db
{
    public class EFDb : DbContext ,IDb
    {
        /*all of this in interface stuff*/
        public List<TUser> TUsers { 
            get {
                return CTUsers.ToList();
            } 
        }
        public bool AddTUser(TUser user)
        {
            CTUsers.Add(user);
            SaveChanges();

            return true;
        }
        public bool ModifyTUser(TUser toBeModified, TUser newData)
        {
            var value = CTUsers.FirstOrDefault(value => value.Id == toBeModified.Id);
            if (value == null) {

                return false;
            }
            if (newData.Nick     != null) value.Nick     = newData.Nick;
            if (newData.Name     != null) value.Name     = newData.Name;
            if (newData.Surname  != null) value.Surname  = newData.Surname;
            if (newData.Password != null) value.Password = newData.Password;

            return true;
        } //todo check for dependecies
        public bool RemoveTUser(TUser user)
        {
            CTUsers.Remove(user);
            SaveChanges();

            return true;
        } //todo check for dependecies

        public List<TUserRole> TUserRoles
        {
            get
            {
                return CTUserRoles.ToList();
            }
        }
        public bool AddTUserRole(TUserRole role)
        {
            CTUserRoles.Add(role);
            SaveChanges();

            return true;
        }
        public bool ModifyTUserRole(TUserRole toBeModified, TUserRole newData)
        {
            var value = CTUserRoles.FirstOrDefault(value => value.Id == toBeModified.Id);
            if (value == null)
            {

                return false;
            }
            if (newData.Name != null) value.Name = newData.Name;

            return true;
        }
        public bool RemoveTUserRole(TUserRole role)
        {
            CTUserRoles.Remove(role);
            SaveChanges();

            return true;
        }

        public List<TItem> TItems
        {
            get
            {
                return CTItems.ToList();
            }
        }
        public bool AddTItem(TItem item)
        {
            throw new NotImplementedException();
        }
        public bool ModifyTItem(TItem toBeModified, TItem newData)
        {
            throw new NotImplementedException();
        }
        public bool RemoveTItem(TItem item)
        {
            throw new NotImplementedException();
        }

        public List<TItemComment> TItemComments
        {
            get
            {
                return CTItemComments.ToList();
            }
        }
        public bool AddTItemComment(TItemComment comment)
        {
            throw new NotImplementedException();
        }
        public bool ModifyTItemComment(TItemComment toBeModified, TItemComment newData)
        {
            throw new NotImplementedException();
        }
        public bool RemoveTItemComment(TItemComment comment)
        {
            throw new NotImplementedException();
        }
        /*end of interface stuff*/

        /*start of EF stuff*/
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TUser> CTUsers { get; set; }
        public DbSet<TUserRole> CTUserRoles { get; set; }
        public DbSet<TItem> CTItems { get; set; }
        public DbSet<TItemComment> CTItemComments { get; set; }
    }
}

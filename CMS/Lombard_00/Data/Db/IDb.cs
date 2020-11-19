using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombard_00.Data.Db
{
    interface IDb
    {
        /*
         * I know that this is violation of several good C# practises but I forgot how to make dependecy injection,
         * so it will do for now.
         */
        private static IDb dbInstance;
        public static IDb DbInstance {
            get {
                if (dbInstance == null)
                    dbInstance = new EFDb();

                return dbInstance;
            }
        }

        /*
         * users - root of all things around here
         */
        public List<TUser> TUsers { get; }
        public TUser AddTUser(TUser user);
        public TUser FindUser(int Id);
        public TUser FindUser(string UniqueNick);
        public bool ModifyTUser(TUser toBeModified, TUser newData);

        /*
         * many to many implementation
         */
        public List<TUserRole> TUserRoles { get; }
        public bool AddTUserRole(TUserRole role);
        public List<TUserRole> FindTUserRoles(int userId);
        public bool RemoveTUserRole(TUserRole role);

        /*
         * each service user can be associated with multiple roles
         * IMPORTANT:
         * role [0] is ALWAYS admin role [1] is ALWAYS user. any custom follow AFTER those.
         * DO *NOT* modify role [0] & [1] as it will break frontend. best don't touch it at all.
         */
        public List<TRole> TRoles { get; }
        public bool AddTRole(TRole role);
        public TRole FindRole(int Id);
        public bool ModifyTRole(TRole toBeModified, TRole newData);

        /*
         * IMPORTANT:
         * DO *NOT* call Modify to add comment OR transaction. it't purpose is to modify local item varables ONLY.
         * for assocaited variables use methods provided:
         * <tbd> jestem zbyt leniwy żeby to dodać jeszcze dziś.
         * 
         * IMPORTANT:
         * TryToFinishDeal zwraca TRUE JEŻELI aukcja się SKOŃCZYŁA i FALSE  jeżeli NIE. to nie świadczy o tym czy operacja zakończyła się sukcesem, z zasady jednak jeżeli nie, zwórcone zostanie FALSE.
         */
        public List<TItem> TItems{ get; }
        public TItem AddTItem(TItem item);
        public bool RemoveTItem(TItem item);
        public bool ModifyTItem(TItem toBeModified, TItem newData);
        public TItem FindTItem(int Id);
        public bool TryToFinishDeal(TItem item);

        /*
         * IMPORTANT:
         * all methods except for GET are *NOT* to be callede manualy.
         * they are here for code provided WITHIN classes above.
         */
        public List<TItemComment> TItemComments { get; }
        public bool AddTItemComment(TItemComment comment);
        public bool RemoveTItemComment(TItemComment comment);
        public bool ModifyTItemComment(TItemComment toBeModified, TItemComment newData);

        /*
         * IMPORTANT: don'ty remove bids that you can reach DIRECTLY from TItem.
         * they are there for purpose.if item is to be sold again create NEW offer (TItem)
         */
        public List<TUserItemBid> TUserItemBids { get; }
        public TUserItemBid AddTUserItemBid(TUserItemBid bid);
        public bool RemoveTUserItemBid(TUserItemBid bid);

        public void CleanUp();
        public void VoidOut();
    }
}

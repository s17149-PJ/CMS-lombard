using Lombard_00.Data.Tables;
using System;
using System.Collections.Generic;

namespace Lombard_00.Data.Db
{
    public interface IDb
    {
        /*
         * I know that this is violation of several good C# practises but I forgot how to make dependecy injection,
         * so it will do for now.
         */
        private static IDb dbInstance;
        public static IDb DbInstance
        {
            get
            {
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
        public TUser FindTUser(int Id);
        public TUser FindTUser(string UniqueNick);
        public bool ModifyTUser(TUser newData);
        public void PokeDbLogin();

        /*
         * many to many implementation
         */
        public bool AddTUserRole(TUser user, TRole role, bool saveAway);
        public bool RemoveTUserRole(TUser user, TRole role, bool saveAway);

        /*
         * each service user can be associated with multiple roles
         * IMPORTANT:
         * role [0] is ALWAYS admin role [1] is ALWAYS user. any custom follow AFTER those.
         * DO *NOT* modify role [0] & [1] as it will break frontend. best don't touch it at all.
         */
        public List<TRole> TRoles { get; }
        public bool AddTRole(TRole role);
        public TRole FindRole(int Id);
        public bool ModifyTRole(TRole newData);

        /*
         * IMPORTANT:
         * DO *NOT* call Modify to add comment OR transaction. it't purpose is to modify local item varables ONLY.
         * for assocaited variables use methods provided:
         * <tbd> jestem zbyt leniwy żeby to dodać jeszcze dziś.
         * 
         * IMPORTANT:
         * TryToFinishDeal zwraca TRUE JEŻELI aukcja się SKOŃCZYŁA i FALSE  jeżeli NIE. to nie świadczy o tym czy operacja zakończyła się sukcesem, z zasady jednak jeżeli nie, zwórcone zostanie FALSE.
         */
        public List<TItem> TItems { get; }
        public TItem AddTItem(TItem item, TUser owner, Decimal Value);
        public bool RemoveTItem(TItem item);
        public bool ModifyTItem(TItem newData);
        public TItem FindTItem(int Id);
        public List<TItem> FindTItems(List<TTag> tags);
        public bool TryToFinishDeal(TItem item);

        /*
         * IMPORTANT:
         * all methods except for GET are *NOT* to be callede manualy.
         * they are here for code provided WITHIN classes above.
         */
        public List<TItemComment> TItemComments { get; }
        public TItemComment AddTItemComment(TItemComment comment);
        public bool RemoveTItemComment(TItemComment comment, bool saveAway);
        public bool ModifyTItemComment(TItemComment newData);
        public TItemComment FindTItemComment(int Id);

        /*
         * IMPORTANT: don'ty remove bids that you can reach DIRECTLY from TItem.
         * they are there for purpose.if item is to be sold again create NEW offer (TItem)
         */
        public List<TUserItemBid> TUserItemBids { get; }
        public TUserItemBid AddTUserItemBid(TUserItemBid bid);
        public bool RemoveTUserItemBid(TUserItemBid bid, bool saveAway);
        public TUserItemBid FindTUserItemBid(int Id);

        /*
         * IMPORTANT: Tags add automatically so don't bother.
         * 
         */
        public List<TTag> TTags { get; }
        public TTag AddTag(TTag tag);
        public bool SoftRemoveTag(TTag tag);
        public TTag FindTag(int Id);
        public TTag HardFindTag(TTag tag);


        public bool AddItemTag(TItem item, TTag tag, bool saveAway);
        public bool RemoveItemTag(TItem item, TTag tag, bool saveAway);

        /*
         * CleanUp -> archive old offers
         */
        public void CleanUp();

        public List<TNode> Log { get; }
    }
}

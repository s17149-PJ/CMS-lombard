﻿using Lombard_00.Data.Tables;
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

        /*users - root of all things around here*/
        public List<TUser> TUsers { get; }
        public bool AddTUser(TUser user);
        public bool RemoveTUser(TUser user);
        public bool ModifyTUser(TUser toBeModified, TUser newData);

        /*each service user can be associated with multiple roles*/
        public List<TUserRole> TUserRoles { get; }
        public bool AddTUserRole(TUserRole role);
        public bool RemoveTUserRole(TUserRole role);
        public bool ModifyTUserRole(TUserRole toBeModified, TUserRole newData);

        /*
         * IMPORTANT:
         * DO *NOT* call Modify to add comment OR transaction. it't purpose is to modify local item varables ONLY.
         * for assocaited variables use methods provided:
         * <tbd> jestem zbyt leniwy żeby to dodać jeszcze dziś.
         */
        public List<TItem> TItems{ get; }
        public bool AddTItem(TItem item);
        public bool RemoveTItem(TItem item);
        public bool ModifyTItem(TItem toBeModified, TItem newData);

        /*
         * IMPORTANT:
         * all methods except for GET are *NOT* to be callede manualy.
         * they are here for code provided WITHIN classes above.
         */
        public List<TItemComment> TItemComments { get; }
        public bool AddTItemComment(TItemComment comment);
        public bool RemoveTItemComment(TItemComment comment);
        public bool ModifyTItemComment(TItemComment toBeModified, TItemComment newData);
    }
}

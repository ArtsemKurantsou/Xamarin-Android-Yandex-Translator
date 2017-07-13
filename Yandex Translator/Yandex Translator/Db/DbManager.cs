using Realms;
using System;
using Yandex_Translator.Models;

namespace Yandex_Translator.Db
{
    class DbManager
    {
        private static RealmConfiguration GetRealmConfiguration()
        {
            RealmConfiguration config = new RealmConfiguration(DbConstants.DB_FILE_NAME);
            config.SchemaVersion = DbConstants.DB_SCHEMA_VERSION;
            config.ObjectClasses = new Type[] { typeof(Translation) };
            config.IsReadOnly = false;
            config.ShouldDeleteIfMigrationNeeded = true;
            return config;
        }
        public static Realm GetRealm()
        {
            return Realm.GetInstance(GetRealmConfiguration());
        }
    }
}
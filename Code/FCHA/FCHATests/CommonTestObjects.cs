using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA.Tests
{
    public static class CommonTestObjects
    {
        private static AccountancyApplication application;
        private static AccountancyDatabaseStub database;

        public static AccountancyApplication Application
        {
            get
            {
                if (null == application)
                    RecreateApplication();
                return application;
            }
        }

        public static void RecreateApplication()
        {
            application = new AccountancyApplication(Database, null);
        }

        public static void RecreateDatabase()
        {
            database = new AccountancyDatabaseStub();
        }

        public static void RecreateAll()
        {
            RecreateDatabase();
            RecreateApplication();
        }

        public static AccountancyDatabaseStub Database
        {
            get
            {
                if (null == database)
                    RecreateDatabase();
                return database;
            }
        }
    }
}

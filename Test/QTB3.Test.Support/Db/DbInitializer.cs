using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace QTB3.Test.Support.Db
{
    public class DbInitializer
    {
        public void Initialize
        (
            SqliteConnection connection,
            Func<SqliteConnection, DbContext> getContext,
            List<IEnumerable<Object>> itemLists
        )
        {
            using (var context = getContext(connection))
            {
                context.Database.EnsureCreated();
                foreach (var itemList in itemLists)
                {
                    context.AddRange(itemList);
                }
                context.SaveChanges();
            }
        }

        public void Initialize
        (
            Func<DbContext> getContext,
            List<IEnumerable<Object>> itemLists
        )
        {
            using (var context = getContext())
            {
                context.Database.EnsureCreated();
                foreach (var itemList in itemLists)
                {
                    context.AddRange(itemList);
                    context.SaveChanges();
                }
            }

        }
    }
}

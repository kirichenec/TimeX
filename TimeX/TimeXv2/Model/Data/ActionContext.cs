using System.Data.Entity;
using System.Data.SQLite.EF6;

namespace TimeXv2.Model.Data
{
    public class ActionContext : DbContext
    {
        public ActionContext() : base(nameof(TimeXv2))
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ActionContext>());
        }

        public DbSet<Action> Actions { get; set; }
    }
}

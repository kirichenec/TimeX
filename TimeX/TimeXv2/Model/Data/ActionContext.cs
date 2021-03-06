﻿using GalaSoft.MvvmLight.Ioc;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace TimeXv2.Model.Data
{
    public class ActionContext : DbContext
    {
        [PreferredConstructor]
        public ActionContext() : base($"{nameof(TimeXv2)}Context") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<ActionContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Action> Actions { get; set; }

        public DbSet<Checkpoint> Checkpoints { get; set; }
    }
}

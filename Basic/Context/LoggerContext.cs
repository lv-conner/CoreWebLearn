using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic.Context
{
    public class LoggerContext:DbContext
    {
        private string connectionString;
        public LoggerContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public LoggerContext():this(null)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(connectionString == null)
            {
                optionsBuilder.UseSqlServer("data Source=PRCNMG1L0311;initial catalog=DBLoggerContext;user id=sa;password=Root@admin;");
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogModelMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}

using Basic.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basic
{
    public class LogModelMap : IEntityTypeConfiguration<LogModel>
    {
        public void Configure(EntityTypeBuilder<LogModel> builder)
        {
            builder.HasKey(p => p.LogId);
            builder.Property(p => p.LogId).ForSqlServerUseSequenceHiLo();
            builder.Property(p => p.Message).IsUnicode();
        }
    }
}

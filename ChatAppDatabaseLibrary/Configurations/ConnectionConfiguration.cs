using ChatAppModelsLibrary.Models.Concrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatAppDatabaseLibrary.Configurations
{
    internal class ConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(EntityTypeBuilder<UserConnection> builder)
        {
            builder.Property(c => c.SofDeleteFrom)
                .HasDefaultValue(false);

            builder.Property(c => c.SoftDeleteTo)
                .HasDefaultValue(false);
        }
    }
}

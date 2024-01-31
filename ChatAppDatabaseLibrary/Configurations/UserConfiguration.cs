using ChatAppModelsLibrary.Models;
using ChatAppModelsLibrary.Models.Concrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDatabaseLibrary.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Password)
                .HasAnnotation("RegularExpression", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()-_+=])[A-Za-z\d!@#$%^&*()-_+=]{8,}$")
                .IsRequired();
            builder.Property(u => u.Bio)
                .HasMaxLength(50);
            builder.HasIndex(u => u.Gmail)
                .IsUnique();

            builder.Property(u => u.Gmail)
                .HasAnnotation("RegularExpression", @"^[a-zA-Z0-9._%+-]+@gmail\.com$");

            //Relations
            builder.HasMany(u=>u.MessagesFroms)
                .WithOne(m=>m.From)
                .HasForeignKey(m=>m.FromId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.MessagesTo)
                .WithOne(m => m.To)
                .HasForeignKey(m => m.ToId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u=>u.ConnectionFroms)
                .WithOne(u=>u.From)
                .HasForeignKey(u=>u.FromId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.ConnectionTos)
               .WithOne(u => u.To)
               .HasForeignKey(u => u.ToId)
               .OnDelete(DeleteBehavior.NoAction);



        }
    }
}

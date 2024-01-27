using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Whatsapp.Models.TestModels;

public partial class MyChatingAppContext : DbContext
{
    public MyChatingAppContext()
    {
    }

    public MyChatingAppContext(DbContextOptions<MyChatingAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MessagesTb> MessagesTbs { get; set; }

    public virtual DbSet<UsersTb> UsersTbs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-47DGCU6\\SQL;Initial Catalog=MyChatingApp;User ID=MySql;Password=pervina9266_1;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessagesTb>(entity =>
        {
            entity.ToTable("MessagesTb");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(50);

            entity.HasOne(d => d.To).WithMany(p => p.MessagesTbTos)
                .HasForeignKey(d => d.ToId)
                .HasConstraintName("CK_ToId_To_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.MessagesTbUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CK_FromId_To_UserId");
        });

        modelBuilder.Entity<UsersTb>(entity =>
        {
            entity.ToTable("UsersTb");

            entity.HasIndex(e => e.Gmail, "Uniqe_Gmail_Constraint").IsUnique();

            entity.HasIndex(e => e.Password, "Uniqe_Password_Constraint").IsUnique();

            entity.Property(e => e.Gmail).HasMaxLength(50);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

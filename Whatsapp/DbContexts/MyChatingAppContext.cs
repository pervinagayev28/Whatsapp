using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Whatsapp.Models;
using System.IO;
using Serilog.Extensions.Logging;

namespace Whatsapp.DbContexts;

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

    public virtual DbSet<UserConnectionsTb> UserConnectionsTbs { get; set; }

    public virtual DbSet<UsersTb> UsersTbs { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=DESKTOP-47DGCU6\\SQL;Database=MyChatingApp;User Id=MySql;Password=pervina9266_1;TrustServerCertificate=True;");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string logFolderPath = Path.Combine(desktopPath, "Logs");
        string logFilePath = Path.Combine(logFolderPath, "Log.txt");

        // Klasörü oluştur
        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }

        // Log dosyasını oluştur
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();

        // Loglama başlangıcı
        Log.Information("Entity Framework Core loglama başladı.");

        optionsBuilder.UseSqlServer("Server=pervin.database.windows.net;Initial Catalog=chatapp;Persist Security Info=False;User ID=agayev;Password=pervina9266_1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog().SetMinimumLevel(LogLevel.Debug)));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessagesTb>(entity =>
        {
            entity.ToTable("MessagesTb");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(50);
        });

        modelBuilder.Entity<UserConnectionsTb>(entity =>
        {
            entity.ToTable("UserConnectionsTb");
        });

        modelBuilder.Entity<UsersTb>(entity =>
        {
            entity.ToTable("UsersTb");

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

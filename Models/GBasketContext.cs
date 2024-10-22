using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace G_Basket_API.Models;

public partial class GBasketContext : DbContext
{
    public GBasketContext()
    {
    }

    public GBasketContext(DbContextOptions<GBasketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardDetail> CardDetails { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=194.60.231.81:5432;Database=G_BASKET;Username=postgres;Password=Maham@7796", x => x.UseNodaTime());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Card_pkey");

            entity.ToTable("Card");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpDate).HasColumnType("time without time zone");
            entity.Property(e => e.ModifyDste).HasColumnType("time without time zone");
            entity.Property(e => e.PayDate).HasColumnType("time without time zone");
            entity.Property(e => e.RegDate).HasColumnType("time without time zone");
        });

        modelBuilder.Entity<CardDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CardDetail_pkey");

            entity.ToTable("CardDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RegDate).HasColumnType("time without time zone");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Status_pkey");

            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Caption).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

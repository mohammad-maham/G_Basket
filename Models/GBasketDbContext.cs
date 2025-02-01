using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace G_Basket_API.Models;

public partial class GBasketDbContext : DbContext
{
    public GBasketDbContext()
    {
    }

    public GBasketDbContext(DbContextOptions<GBasketDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardDetail> CardDetails { get; set; }

    public virtual DbSet<DeliveryMode> DeliveryModes { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<PeymentMode> PeymentModes { get; set; }

    public virtual DbSet<PeymentTransaction> PeymentTransactions { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TestTable> TestTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=194.60.231.11:5432;Database=G_Basket_DB;Username=postgres;Password=7796", x => x.UseNodaTime());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Card_pkey");

            entity.ToTable("Card");

            entity.Property(e => e.DiliveryInfo).HasColumnType("json");
            entity.Property(e => e.DiliveryModeId).HasDefaultValue(102);
            entity.Property(e => e.PayInvoiceId).HasDefaultValue(0L);
            entity.Property(e => e.PeymentInfo).HasColumnType("json");
            entity.Property(e => e.PeymentModeId).HasDefaultValue(101);
        });

        modelBuilder.Entity<CardDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CardDetail_pkey");

            entity.ToTable("CardDetail");
        });

        modelBuilder.Entity<DeliveryMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DeliveryMode_pkey");

            entity.ToTable("DeliveryMode");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Invoice_pkey");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceDetail).HasColumnType("json");
            entity.Property(e => e.RegDate).HasColumnType("time without time zone");
        });

        modelBuilder.Entity<PeymentMode>(entity =>
        {
            entity.HasKey(e => e.PrimaryKey).HasName("PeymentMode_pkey");

            entity.ToTable("PeymentMode");

            entity.Property(e => e.PrimaryKey).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<PeymentTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PeymentTransaction_pkey");

            entity.ToTable("PeymentTransaction");

            entity.Property(e => e.Id).HasIdentityOptions(null, null, null, 2147483647L, null, null);
            entity.Property(e => e.PeymentInfo).HasColumnType("json");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Status_pkey");

            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Caption).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TestTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Id");

            entity.ToTable("TestTable", "Test");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });
        modelBuilder.HasSequence("CardDetailSequence");
        modelBuilder.HasSequence("CardSequence");
        modelBuilder.HasSequence("InvoiceSeq");
        modelBuilder.HasSequence("PeymentModeSequence");
        modelBuilder.HasSequence("PeymentTransactionSq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

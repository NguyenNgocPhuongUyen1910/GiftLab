using System;
using System.Collections.Generic;
using GiftLab.Data.Entities;
using Microsoft.EntityFrameworkCore;

// ✅ FIX: tránh trùng giữa GiftLab.Data.Entities.Attribute và System.Attribute
using DbAttribute = GiftLab.Data.Entities.Attribute;

namespace GiftLab.Data;

public partial class GiftLabDbContext : DbContext
{
    public GiftLabDbContext()
    {
    }

    public GiftLabDbContext(DbContextOptions<GiftLabDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    // ✅ FIX: dùng alias DbAttribute
    public virtual DbSet<DbAttribute> Attributes { get; set; }

    public virtual DbSet<AttributesPrice> AttributesPrices { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TransacStatus> TransacStatuses { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccountID).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts).HasConstraintName("FK_Accounts_Roles");
        });

        // ✅ FIX: dùng alias DbAttribute
        modelBuilder.Entity<DbAttribute>(entity =>
        {
            entity.Property(e => e.AttributeID).ValueGeneratedNever();
        });

        modelBuilder.Entity<AttributesPrice>(entity =>
        {
            entity.Property(e => e.AttributesPriceID).ValueGeneratedNever();

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributesPrices).HasConstraintName("FK_AttributesPrices_Attributes");

            entity.HasOne(d => d.Product).WithMany(p => p.AttributesPrices).HasConstraintName("FK_AttributesPrices_Products");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CatID).ValueGeneratedNever();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Email).IsFixedLength();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.TransactionStatus).WithMany(p => p.Orders).HasConstraintName("FK_Orders_TransacStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.OrderDetailID).ValueGeneratedNever();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("FK_OrderDetails_Orders");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Cat).WithMany(p => p.Products).HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleID).ValueGeneratedNever();
        });

        modelBuilder.Entity<TransacStatus>(entity =>
        {
            entity.Property(e => e.TransactStatusID).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

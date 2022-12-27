using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppAspNetCore.Models;

public partial class Resbooking1Context : DbContext
{
    public Resbooking1Context()
    {
    }

    public Resbooking1Context(DbContextOptions<Resbooking1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealCategory> MealCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<TableType> TableTypes { get; set; }

    public virtual DbSet<TransactStatus> TransactStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=resbooking1;Persist Security Info=False;User ID=sa;Password=Password.1;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.CreateBy).HasMaxLength(100);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LastLoginTime).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(36);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.UpdateBy).HasMaxLength(100);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(100);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.LastLoginTime).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(36);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.Salt)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.UpdateBy).HasMaxLength(100);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.Ward).HasMaxLength(100);

            entity.HasOne(d => d.Location).WithMany(p => p.Customers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Customer_Location");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.ToTable("Meal");

            entity.Property(e => e.CreateBy).HasMaxLength(100);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(100);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.MealCategory).WithMany(p => p.Meals)
                .HasForeignKey(d => d.MealCategoryId)
                .HasConstraintName("FK_Meal_MealCategory");
        });

        modelBuilder.Entity<MealCategory>(entity =>
        {
            entity.ToTable("MealCategory");

            entity.Property(e => e.CreateBy).HasMaxLength(100);
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdateBy).HasMaxLength(100);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDay).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.Meal).WithMany(p => p.Orders)
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK_Order_Meal");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK_Order_Table");

            entity.HasOne(d => d.TransactStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TransactStatusId)
                .HasConstraintName("FK_Order_TransactStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.HasOne(d => d.Meal).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MealId)
                .HasConstraintName("FK_OrderDetail_Meal");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.ToTable("Table");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.TableType).WithMany(p => p.Tables)
                .HasForeignKey(d => d.TableTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Table_TableType");
        });

        modelBuilder.Entity<TableType>(entity =>
        {
            entity.ToTable("TableType");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransactStatus>(entity =>
        {
            entity.ToTable("TransactStatus");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

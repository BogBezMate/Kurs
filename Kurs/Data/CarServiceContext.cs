using Microsoft.EntityFrameworkCore;
using Kurs.Models;

namespace Kurs.Data
{
    public class CarServiceContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderPart> WorkOrderParts { get; set; }
        public DbSet<WorkOrderService> WorkOrderServices { get; set; }
        public DbSet<WorkOrderEmployee> WorkOrderEmployees { get; set; }

        // Конструктор без параметров — для обычного использования в приложении
        public CarServiceContext() { }

        // Конструктор с параметрами — для EF Core Tools (миграции)
        public CarServiceContext(DbContextOptions<CarServiceContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Срабатывает только если опции ещё не заданы (т.е. при вызове без параметров)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=CarServiceDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee
            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(x => x.ID_Employees);
                e.Property(x => x.Last_Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.First_Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.Middle_Name).HasMaxLength(50);
                e.Property(x => x.Position).IsRequired().HasMaxLength(50);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.Email).HasMaxLength(100);
                // Computed properties — ignore
                e.Ignore(x => x.FullName);
                e.Ignore(x => x.ShortName);
            });

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.ID_Users);
                e.Property(x => x.Username).IsRequired().HasMaxLength(50);
                e.Property(x => x.Password).IsRequired().HasMaxLength(100);
                e.HasOne(x => x.Employee)
                 .WithOne(x => x.User)
                 .HasForeignKey<User>(x => x.ID_Employees);
            });

            // Supplier
            modelBuilder.Entity<Supplier>(e =>
            {
                e.HasKey(x => x.ID_Suppliers);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.Email).HasMaxLength(100);
                e.Property(x => x.Contact_Name).HasMaxLength(100);
            });

            // Part
            modelBuilder.Entity<Part>(e =>
            {
                e.HasKey(x => x.ID_Parts);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Quantity).HasDefaultValue(0);
                e.Property(x => x.Price).HasColumnType("decimal(10,2)").HasDefaultValue(0);
                e.HasOne(x => x.Supplier)
                 .WithMany(x => x.Parts)
                 .HasForeignKey(x => x.ID_Suppliers)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // Customer
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(x => x.ID_Customers);
                e.Property(x => x.Last_Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.First_Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.Middle_Name).HasMaxLength(50);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Ignore(x => x.FullName);
            });

            // Vehicle
            modelBuilder.Entity<Vehicle>(e =>
            {
                e.HasKey(x => x.ID_Vehicles);
                e.Property(x => x.VIN).IsRequired().HasMaxLength(17);
                e.HasIndex(x => x.VIN).IsUnique();
                e.Property(x => x.Plate_Number).IsRequired().HasMaxLength(20);
                e.Property(x => x.Brand).HasMaxLength(50);
                e.Property(x => x.Model).HasMaxLength(50);
                e.HasOne(x => x.Customer)
                 .WithMany(x => x.Vehicles)
                 .HasForeignKey(x => x.ID_Customers);
                e.Ignore(x => x.DisplayName);
            });

            // WorkOrder
            modelBuilder.Entity<WorkOrder>(e =>
            {
                e.HasKey(x => x.ID_WorkOrders);
                e.Property(x => x.Number).IsRequired().HasMaxLength(20);
                e.HasIndex(x => x.Number).IsUnique();
                e.Property(x => x.Status).HasMaxLength(30).HasDefaultValue("Открыт");
                e.Property(x => x.Notes).HasMaxLength(500);
                e.HasOne(x => x.Customer)
                 .WithMany(x => x.WorkOrders)
                 .HasForeignKey(x => x.ID_Customers)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Vehicle)
                 .WithMany(x => x.WorkOrders)
                 .HasForeignKey(x => x.ID_Vehicles)
                 .OnDelete(DeleteBehavior.Restrict);
                // Computed properties — ignore
                e.Ignore(x => x.TotalParts);
                e.Ignore(x => x.TotalServices);
                e.Ignore(x => x.GrandTotal);
            });

            // WorkOrderPart
            modelBuilder.Entity<WorkOrderPart>(e =>
            {
                e.HasKey(x => x.ID_WorkOrderParts);
                e.Property(x => x.Unit_Price).HasColumnType("decimal(10,2)");
                e.HasOne(x => x.WorkOrder)
                 .WithMany(x => x.WorkOrderParts)
                 .HasForeignKey(x => x.ID_WorkOrders);
                e.HasOne(x => x.Part)
                 .WithMany(x => x.WorkOrderParts)
                 .HasForeignKey(x => x.ID_Parts)
                 .OnDelete(DeleteBehavior.Restrict);
                e.Ignore(x => x.LineSum);
            });

            // WorkOrderService
            modelBuilder.Entity<WorkOrderService>(e =>
            {
                e.HasKey(x => x.ID_WorkOrderServices);
                e.Property(x => x.Description).IsRequired().HasMaxLength(200);
                e.Property(x => x.Price).HasColumnType("decimal(10,2)");
                e.HasOne(x => x.WorkOrder)
                 .WithMany(x => x.WorkOrderServices)
                 .HasForeignKey(x => x.ID_WorkOrders);
            });

            // WorkOrderEmployee
            modelBuilder.Entity<WorkOrderEmployee>(e =>
            {
                e.HasKey(x => x.ID_WorkOrderEmployees);
                e.HasIndex(x => new { x.ID_WorkOrders, x.ID_Employees }).IsUnique();
                e.HasOne(x => x.WorkOrder)
                 .WithMany(x => x.WorkOrderEmployees)
                 .HasForeignKey(x => x.ID_WorkOrders);
                e.HasOne(x => x.Employee)
                 .WithMany(x => x.WorkOrderEmployees)
                 .HasForeignKey(x => x.ID_Employees)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

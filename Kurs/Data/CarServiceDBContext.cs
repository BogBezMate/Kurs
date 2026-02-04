using Microsoft.EntityFrameworkCore;
using Kurs.Models;


public class CarServiceContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderPart> WorkOrderParts { get; set; }
    public DbSet<WorkOrderService> WorkOrderServices { get; set; }
    public DbSet<WorkOrderEmployee> WorkOrderEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=HOME-PC\\SQLEXPRESS;Database=CarServiceDB;Trusted_Connection=True;TrustServerCertificate=True");
    }
}

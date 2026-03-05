namespace Kurs.Models
{
    public class Customer
    {
        public int ID_Customers { get; set; }
        public string Last_Name { get; set; } = "";
        public string First_Name { get; set; } = "";
        public string? Middle_Name { get; set; }
        public string? Phone { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

        public string FullName => $"{Last_Name} {First_Name} {Middle_Name}".Trim();
    }
}

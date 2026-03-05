namespace Kurs.Models
{
    public class WorkOrder
    {
        public int ID_WorkOrders { get; set; }
        public string Number { get; set; } = "";
        public int ID_Customers { get; set; }
        public int ID_Vehicles { get; set; }
        public DateTime Date_Open { get; set; } = DateTime.Today;
        public DateTime? Date_Close { get; set; }
        public string Status { get; set; } = "Открыт";
        public string? Notes { get; set; }

        public Customer Customer { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
        public ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();
        public ICollection<WorkOrderService> WorkOrderServices { get; set; } = new List<WorkOrderService>();
        public ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; } = new List<WorkOrderEmployee>();

        // Вычисляемые свойства — не хранятся в БД
        public decimal TotalParts => WorkOrderParts.Sum(p => p.Quantity * p.Unit_Price);
        public decimal TotalServices => WorkOrderServices.Sum(s => s.Price);
        public decimal GrandTotal => TotalParts + TotalServices;
    }
}

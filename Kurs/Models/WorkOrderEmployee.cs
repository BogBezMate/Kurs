namespace Kurs.Models
{
    public class WorkOrderEmployee
    {
        public int ID_WorkOrderEmployees { get; set; }
        public int ID_WorkOrders { get; set; }
        public int ID_Employees { get; set; }

        public WorkOrder WorkOrder { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}

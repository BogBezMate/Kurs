namespace Kurs.Models
{
    public class WorkOrderPart
    {
        public int ID_WorkOrderParts { get; set; }
        public int ID_WorkOrders { get; set; }
        public int ID_Parts { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }

        public WorkOrder WorkOrder { get; set; } = null!;
        public Part Part { get; set; } = null!;

        // Вычисляемое — не хранится в БД
        public decimal LineSum => Quantity * Unit_Price;
    }
}

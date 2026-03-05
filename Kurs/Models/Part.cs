namespace Kurs.Models
{
    public class Part
    {
        public int ID_Parts { get; set; }
        public int? ID_Suppliers { get; set; }
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Supplier? Supplier { get; set; }
        public ICollection<WorkOrderPart> WorkOrderParts { get; set; } = new List<WorkOrderPart>();
    }
}

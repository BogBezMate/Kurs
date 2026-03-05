namespace Kurs.Models
{
    public class Supplier
    {
        public int ID_Suppliers { get; set; }
        public string Name { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Contact_Name { get; set; }

        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}

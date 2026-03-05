namespace Kurs.Models
{
    public class Employee
    {
        public int ID_Employees { get; set; }
        public string Last_Name { get; set; } = "";
        public string First_Name { get; set; } = "";
        public string? Middle_Name { get; set; }
        public string Position { get; set; } = "";
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public User? User { get; set; }
        public ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; } = new List<WorkOrderEmployee>();

        public string FullName => $"{Last_Name} {First_Name} {Middle_Name}".Trim();
        public string ShortName => $"{Last_Name} {First_Name?[0]}. {(Middle_Name != null ? Middle_Name[0] + "." : "")}".Trim();
    }
}

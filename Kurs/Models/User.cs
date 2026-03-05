namespace Kurs.Models
{
    public class User
    {
        public int ID_Users { get; set; }
        public int ID_Employees { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public Employee Employee { get; set; } = null!;
    }
}

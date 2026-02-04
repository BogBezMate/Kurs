using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Kurs.Models
{
    public class Employee
    {
        public int ID_Employees { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; }
    }
}



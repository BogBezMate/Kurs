using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kurs.Models
{
    public class Customer
    {
        public int ID_Customers { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Phone { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}


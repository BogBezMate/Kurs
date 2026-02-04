using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kurs.Models
{
    public class Vehicle
    {
        public int ID_Vehicles { get; set; }
        public int ID_Customers { get; set; }
        public string VIN { get; set; }
        public string Plate_Number { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }

        public Customer Customer { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}


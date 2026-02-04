using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kurs.Models
{
    public class WorkOrder
    {
        public int ID_WorkOrders { get; set; }
        public string Number { get; set; }
        public int ID_Customers { get; set; }
        public int ID_Vehicles { get; set; }
        public DateTime Date_Open { get; set; }
        public string Status { get; set; }
        public decimal? Total_Parts { get; set; }
        public decimal? Total_Services { get; set; }
        public decimal? Total_Amount { get; set; }

        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }

        public ICollection<WorkOrderPart> WorkOrderParts { get; set; }
        public ICollection<WorkOrderService> WorkOrderServices { get; set; }
        public ICollection<WorkOrderEmployee> WorkOrderEmployees { get; set; }
    }
}


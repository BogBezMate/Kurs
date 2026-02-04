using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kurs.Models
{
    public class WorkOrderService
    {
        public int ID_WorkOrderServices { get; set; }
        public int ID_WorkOrders { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public WorkOrder WorkOrder { get; set; }
    }
}


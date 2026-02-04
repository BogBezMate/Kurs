using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kurs.Models
{
    public class WorkOrderPart
    {
        public int ID_WorkOrderParts { get; set; }
        public int ID_WorkOrders { get; set; }
        public int ID_Parts { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal Line_Sum { get; set; }

        public WorkOrder WorkOrder { get; set; } 
        public Part Part { get; set; }          
    }
}


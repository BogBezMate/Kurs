using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Kurs.Models
{
    public class WorkOrderEmployee
    {
        public int ID_WorkOrderEmployees { get; set; }
        public int ID_WorkOrders { get; set; }
        public int ID_Employees { get; set; }

        public WorkOrder WorkOrder { get; set; }
        public Employee Employee { get; set; }
    }
}

}


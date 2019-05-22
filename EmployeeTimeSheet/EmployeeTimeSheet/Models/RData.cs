using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Models
{
    public class RData
    {
        public DateTime startDate { get; set; }
      
        public string approvalStatus { get; set; }
        public DateTime endDate { get; set; }
        public string timeType { get; set; }
    }
}

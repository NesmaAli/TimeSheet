using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.Models
{
    public class Result
    {
        public Metadata __metadata { get; set; }
        public DateTime startDate { get; set; }
        public string userId { get; set; }
        public string approvalStatus { get; set; }
        public DateTime endDate { get; set; }
        public string timeType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Dtos
{
    public class UserEarningDto
    {
        public decimal TotalEaning { get; set; }
        public decimal TotalTaskCompleted { get; set; }
        public decimal TotalTaskFailed { get; set; }
    }
}

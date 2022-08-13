using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Data.Interfaces
{
    public interface ITracking
    {
        public bool IsDeleted { get; set; }
        DateTime CreatedDate { get; set; }

        DateTime? UpdatedDate { get; set; }

        string CreatedBy { get; set; }

        string UpdatedBy { get; set; }
    }
}

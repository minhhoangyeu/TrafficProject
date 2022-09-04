using System.ComponentModel;

namespace Traffic.Utilities
{
    public class Enums
    {
        public enum DoTaskStatus
        {
            Open,
            Processing,
            Completed,
            Failed
        }
        public enum UserStatus
        {
            Pending = 0,
            Activated,
            DeActivated,
            Locked,
            IsSuperAdmin
        }
        public enum CampaignStatus
        {
            New =0,
            Pending,
            Canceled,
            Closed,
            Approved,
            Rejected,
        }
    }
}

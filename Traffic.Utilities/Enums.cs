using System.ComponentModel;

namespace Traffic.Utilities
{
    public class Enums
    {
        public enum Status
        {
            SUCCESS,
            ERROR,
            DELIVERED
        }
        public enum Result
        {
            SUCCESS,
            UNAUTHORIZED,
            FORBIDDEN,
            ERROR
        }
        public enum Severity
        {
            HIGH,
            MEDIUM,
            LOW
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

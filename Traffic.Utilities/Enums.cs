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

        public enum SendingStatus
        {
            UNSENT = 1,
            SENDING = 2,
            [Description("Skip send SMS")]
            ARCHIVED = 3,
            FAILED = 4,
            SUCCESS = 5,
            ALL = -1
        }

        public enum OperationStatus
        {
            FIRST_DELIVERY = 1,
            IN_WAREHOUSE = 2,
            RE_DELIVERY = 3,
            LOST = 4,
            DESTRUCT_PLASTIC_CARD = 5,
            PREPARE_TO_DELIVERY = 6
        }

        public enum DeliveryStatus
        {
           READY_FOR_DELIVERY = 70,
           UNSUCCESSFUL = 91,
           SUCCESSFUL = 100,
           RETURN_SENDER_UNSUCCESSFUL = 161,
           RETURN_SENDER_SUCCESSFUL = 170
        }

        public enum CompareSymbolStatus
        {
            SMAILER_OR_EQUAL = -1,
            EQUAL = 0, 
            BIGGER_OR_EQUAL = 1,
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

        public enum CardType
        {
            N2,
            N4
        }

        public enum StatisticType
        {
            ISSUE_CARDS_BY_DATE = 1,
            ISSUE_CARDS_BY_PROVINCE = 2,
            REDELIVERY_BY_DATE = 3
        }
    }
}

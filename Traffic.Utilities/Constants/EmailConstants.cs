using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Utilities.Constants
{
    public class EmailConstants
    {
        public class VNPOST
        {
            public const string Recipients = "hoang.nguyenn@homecredit.vn;Linh.NguyenTN3@homecredit.vn";

            public const string MessageCode = "Traffic_EMAIL_NOTIFICATION";

            public const string Subject = "NON-POSTCODE CARD DELIVERY";

            public const string Content = "Dear VNPOST,<br><br>There are <b>{total_count}</b> cards have no Postcode. Please access this link <a href='https://partner.homecredit.vn/'>partner.homecredit.vn</a> to find out PCID list.<br>(<i>Note: Total PCID in partner portal may be less or equal total cards are counted</i>)<p>{table_data}<br><br>Best Regards,<br>Home Credit VN";
        }
    }
}

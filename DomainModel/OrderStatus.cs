using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModel
{
    public class OrderStatus
    {
        private static readonly OrderStatus _instance = new OrderStatus();

        private readonly IDictionary<string, string> _statusses;

        private OrderStatus()
        {
            _statusses = new Dictionary<string, string>
                {
                    {"0", "Incomplete or invalid"},
                    {"1", "Cancelled by customer"},
                    {"2", "Authorization declined"},
                    {"4", "Order stored"},
                    {"5", "Authorised"},
                    {"6", "Authorised and cancelled"},
                    {"7", "Payment deleted"},
                    {"8", "Refund"},
                    {"9", "Payment requested"},
                    {"40", "Stored waiting external result"},
                    {"41", "Waiting for client payment"},
                    {"50", "Authorized waiting external result"},
                    {"51", "Authorisation waiting"},
                    {"52", "Authorisation not known"},
                    {"55", "Standby"},
                    {"56", "OK with scheduled payments"},
                    {"57", "Not OK with scheduled payments"},
                    {"59", "Authoris. to be requested manually"},
                    {"61", "Author. deletion waiting"},
                    {"62", "Author. deletion uncertain"},
                    {"63", "Author. deletion refused"},
                    {"64", "Authorised and cancelled"},
                    {"71", "Payment deletion pending"},
                    {"72", "Payment deletion uncertain"},
                    {"73", "Payment deletion refused"},
                    {"74", "Payment deleted"},
                    {"75", "Deletion processed by merchant"},
                    {"81", "Refund pending"},
                    {"82", "Refund uncertain"},
                    {"83", "Refund refused"},
                    {"84", "Payment declined by the acquirer"},
                    {"85", "Refund processed by merchant"},
                    {"91", "Payment processing"},
                    {"92", "Payment uncertain"},
                    {"93", "Payment refused"},
                    {"94", "Refund declined by the acquirer"},
                    {"95", "Payment processed by merchant"},
                    {"96", "Refund reversed"},
                    {"99", "Being processed"},
                };
        }

        public static OrderStatus Instance { get { return _instance; } }

        public string this [string index] => string.IsNullOrEmpty(index) || !_statusses.ContainsKey(index) ? index : _statusses[index];
    }
}

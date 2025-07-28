using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities
{
    public enum OrderStatus
    {
        Pending = 0,
        AwaitingPayment = 1,
        PaymentFailed = 2,
        PaymentSucceeded = 3,
        Shipped = 4,
        Delivered = 5,
        Canceled = 6,
        Refunded = 7,
        Completed = 8,
        Failed = 9,
        Processing = 10,
        Rejected = 11
    }
}

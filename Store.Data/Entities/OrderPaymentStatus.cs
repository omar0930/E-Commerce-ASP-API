using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Entities
{
    public enum OrderPaymentStatus
    {
        Pending = 0,
        AwaitingPayment = 1,
        PaymentFailed = 2,
        PaymentSucceeded = 3,
        Canceled = 4,
    }
}

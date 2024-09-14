using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.OrderEntity
{
    public  enum OrderStatus
    {
        Pending,
        PaymentRecived,
        PaymentFaild,
        Accept
    }
}

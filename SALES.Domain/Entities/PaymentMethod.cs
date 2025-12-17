using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class PaymentMethod
{
    public int IdPaymentMethod { get; set; }
    public string MethodName { get; set; } = null!;

    public virtual ICollection<SalePayment> SalePayments { get; set; } = new List<SalePayment>();
}

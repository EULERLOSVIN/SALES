using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class SalePayment
{
    public int IdSalePayment { get; set; }
    public int IdSaleOrder { get; set; }
    public int IdPaymentMethod { get; set; }
    public int IdCart { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaymentDate { get; set; }

    public virtual PaymentMethod IdPaymentMethodNavigation { get; set; } = null!;
    public virtual SaleOrder IdSaleOrderNavigation { get; set; } = null!;
}

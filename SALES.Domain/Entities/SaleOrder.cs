using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class SaleOrder
{
    public int IdSaleOrder { get; set; }
    public int IdUserAccount { get; set; }
    public int IdSaleState { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? TrackingNumber { get; set; }

    public virtual SaleState IdSaleStateNavigation { get; set; } = null!;
    public virtual ICollection<SalePayment> SalePayments { get; set; } = new List<SalePayment>();
    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();
}

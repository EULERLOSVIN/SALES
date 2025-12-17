using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class SaleState
{
    public int IdSaleState { get; set; }
    public string StateName { get; set; } = null!;

    public virtual ICollection<SaleOrder> SaleOrders { get; set; } = new List<SaleOrder>();
}

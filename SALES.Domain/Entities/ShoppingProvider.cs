using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class ShoppingProvider
{
    public int IdShoppingProvider { get; set; }
    public string NameProvider { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();
}

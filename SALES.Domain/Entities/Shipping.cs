using System;
using System.Collections.Generic;

namespace SALES.Persistence;

public partial class Shipping
{
    public int IdShipping { get; set; }
    public int IdSaleOrder { get; set; }
    public string? ReceiverName { get; set; }
    public string? ReceiverLastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Dni { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? PostalCode { get; set; }
    public int IdShoppingProvider { get; set; }
    public string AddressReference { get; set; } = null!;

    public virtual SaleOrder IdSaleOrderNavigation { get; set; } = null!;
    public virtual ShoppingProvider IdShoppingProviderNavigation { get; set; } = null!;
}

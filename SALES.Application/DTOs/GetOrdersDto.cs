
namespace SALES.Application.DTOs
{
    public class OrderDto
    {
        public int IdOrder { get; set; }
        public required string TrackingNumber { get; set; }
        public required string State { get; set; }
        public DateTime? DateTime { get; set; }
        public required DetailShipping DetailShipping { get; set; }
        public required SalePaymentDto SalePayment { get; set; }
    }
    
    public class DetailShipping
    {
        public int IdDetailShipping { get; set; }
        public required string ReceiverName { get; set; }
        public required string RceiverLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Dni {  get; set; }
        public required string City { get; set; }
        public required string Region { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public required string AddressReference { get; set; }
        public required ShippingProviderDTo Provider { get; set; }
    }

    public class ShippingProviderDTo
    {
        public int IdShoppingProvider { get; set; }
        public required string NameProvider { get; set; }
    }

    public class SalePaymentDto {
        public int IdSalePayment { get; set; }
        public int IdCart { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
    }
}

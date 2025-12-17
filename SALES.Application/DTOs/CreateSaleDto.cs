namespace SALES.Application.DTOs
{
    public class CreateSaleDto
    {
        public int IdUserAccount { get; set; }
        public int IdCart { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }

        public required string ReceiverName { get; set; }
        public required string ReceiverLastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Dni { get; set; }
        public required string City { get; set; }
        public required string Region { get; set; }
        public required string Province { get; set; }
        public required string District { get; set; }
        public required string PostalCode { get; set; }


        public required string AddressReference { get; set; }
        public int IdShoppingProvider { get; set; }

        public int IdPaymentMethod { get; set; }
        public string? CardNumber { get; set; }     
        public string? CardHolderName { get; set; } 
        public string? CardExpiration { get; set; }  
        public string? CardCvv { get; set; }

        public List<SaleItemDto>? SaleItems { get; set; }
    }
}
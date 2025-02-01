namespace G_Basket_API.ViewModels;

public class PaymentCardRequest
{
    public long CardId { get; set; }
    public decimal Amount { get; set; }
    public decimal? AmountByVat { get; set; }
    public long? WalletId { get; set; }
    public string PaymentInfo { get; set; }
    public string DeliveryInfo { get; set; }
    public long PayInvoiceId { get; set; }
}
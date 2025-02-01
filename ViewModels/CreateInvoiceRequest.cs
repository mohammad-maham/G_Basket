namespace G_Basket_API.ViewModels;

public class CreateInvoiceRequest
{
    public long cardId { get; set; }
    public int peyEntityId{ get; set; }
    public string invoiceDetail{ get; set; }
}
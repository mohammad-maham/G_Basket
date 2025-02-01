using System;
using System.Collections.Generic;
using NodaTime;

namespace G_Basket_API.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public int PeyEntityId { get; set; }

    public string InvoiceDetail { get; set; } = null!;

    public int Status { get; set; }

    public LocalTime RegDate { get; set; }
}

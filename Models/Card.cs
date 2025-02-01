using System;
using System.Collections.Generic;
using NodaTime;

namespace G_Basket_API.Models;

public partial class Card
{
    public long Id { get; set; }

    public short Status { get; set; }

    public long UserId { get; set; }

    public decimal? Amount { get; set; }

    public decimal? AmountByVat { get; set; }

    public long? WalletId { get; set; }

    public Instant? ModifyDate { get; set; }

    public Instant? ExpDate { get; set; }

    public Instant? RegDate { get; set; }

    public Instant? PayDate { get; set; }

    public int DiliveryModeId { get; set; }

    public int PeymentModeId { get; set; }

    public string? PeymentInfo { get; set; }

    public long PayInvoiceId { get; set; }

    public string? DiliveryInfo { get; set; }
}

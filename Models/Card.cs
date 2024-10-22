using System;
using System.Collections.Generic;
using NodaTime;

namespace G_Basket_API.Models;

public partial class Card
{
    public long Id { get; set; }

    public short Status { get; set; }

    public LocalTime RegDate { get; set; }

    public long UserId { get; set; }

    public LocalTime ExpDate { get; set; }

    public decimal Amount { get; set; }

    public decimal AmountByVat { get; set; }

    public long? WalletId { get; set; }

    public LocalTime? ModifyDste { get; set; }

    public LocalTime? PayDate { get; set; }
}

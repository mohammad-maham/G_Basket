using System;
using System.Collections.Generic;
using NodaTime;

namespace G_Basket_API.Models;

public partial class CardDetail
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public long ProductId { get; set; }

    public short Status { get; set; }

    public decimal Amount { get; set; }

    public Instant RegDate { get; set; }

    public int EntityId { get; set; }
}

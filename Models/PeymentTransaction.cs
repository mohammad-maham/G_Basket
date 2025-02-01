using System;
using System.Collections.Generic;

namespace G_Basket_API.Models;

public partial class PeymentTransaction
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public string PeymentInfo { get; set; } = null!;

    public int? Status { get; set; }

    public int PeymentMode { get; set; }
}

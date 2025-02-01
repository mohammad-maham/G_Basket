using System;
using System.Collections.Generic;

namespace G_Basket_API.Models;

public partial class DeliveryMode
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public string Title { get; set; } = null!;
}

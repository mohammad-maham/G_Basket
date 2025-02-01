using System;
using System.Collections.Generic;
using NodaTime;

namespace G_Basket_API.Models;

public partial class PeymentMode
{
    public Period Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Period Status { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int PrimaryKey { get; set; }
}

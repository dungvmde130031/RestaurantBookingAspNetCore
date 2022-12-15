using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Table
{
    public int TableId { get; set; }

    public int TableTypeId { get; set; }

    public int? TableNumber { get; set; }

    public int? Name { get; set; }

    public int? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual TableType TableType { get; set; } = null!;
}

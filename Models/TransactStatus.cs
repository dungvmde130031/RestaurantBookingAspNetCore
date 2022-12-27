using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class TransactStatus
{
    public int TransactStatusId { get; set; }

    public bool? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}

using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}

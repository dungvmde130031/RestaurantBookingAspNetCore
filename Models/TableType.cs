using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class TableType
{
    public int TableTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Table> Tables { get; } = new List<Table>();
}

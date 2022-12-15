using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? MealId { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public int? Total { get; set; }

    public virtual Meal? Meal { get; set; }

    public virtual Order? Order { get; set; }
}

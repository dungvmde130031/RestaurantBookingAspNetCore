using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Meal
{
    public int MealId { get; set; }

    public int? MealCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public int Price { get; set; }

    public string? Image { get; set; }

    public bool? IsFavorite { get; set; }

    public bool? Status { get; set; }

    public int? Discount { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? UpdateBy { get; set; }

    public virtual MealCategory? MealCategory { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}

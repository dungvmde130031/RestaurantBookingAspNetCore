using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class MealCategory
{
    public int MealCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Title { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? UpdateBy { get; set; }

    public virtual ICollection<Meal> Meals { get; } = new List<Meal>();
}

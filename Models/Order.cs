using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? TableId { get; set; }

    public int? MealId { get; set; }

    public int? TransactStatusId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? PaymentDay { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Note { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Meal? Meal { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Table? Table { get; set; }

    public virtual TransactStatus? TransactStatus { get; set; }
}

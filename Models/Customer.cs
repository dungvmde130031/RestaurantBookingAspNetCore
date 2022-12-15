using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int? LocationId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? UpdateBy { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}

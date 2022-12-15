using System;
using System.Collections.Generic;

namespace AppAspNetCore.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public DateTime? CreateTime { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? UpdateBy { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}

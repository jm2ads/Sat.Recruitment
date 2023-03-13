using System;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string UserType { get; set; } = null!;

    public decimal? Money { get; set; }
}

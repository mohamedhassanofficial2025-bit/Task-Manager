using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Infrastructure.Data;

public class AppRole : IdentityRole
{
    public string Description { get; set; } = string.Empty;
}

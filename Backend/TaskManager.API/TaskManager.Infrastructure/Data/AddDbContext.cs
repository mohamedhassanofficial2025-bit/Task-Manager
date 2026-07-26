using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TaskManager.Core.Entities;
using System.Text;

namespace TaskManager.Infrastructure.Data;

public class AddDbContext : DbContext
{
    public AddDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
}


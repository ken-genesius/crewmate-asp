﻿using CrewMate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrewMate.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach(var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<LeaveApplication>()
            .HasOne(f => f.Status)
            .WithMany()
            .HasForeignKey(f => f.StatusId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Designation> Designations{ get; set; }

    public DbSet<Bank> Banks { get; set; }

    public DbSet<SystemCode> SystemCodes { get; set; }

    public DbSet<SystemCodeDetail> SystemCodeDetails { get; set; }

    public DbSet<LeaveType> LeaveTypes { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<City> Cities { get; set; }

    public DbSet<LeaveApplication> LeaveApplications { get; set; }
}

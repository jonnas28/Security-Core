using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Payroll.Core.Model;
using System.Xml;

namespace Payroll.Core.Context
{
    public partial class PayrollContext : DbContext, IDisposable
    {
        IConfiguration _configuration;
        public PayrollContext(DbContextOptions<PayrollContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_configuration.GetConnectionString("PayrollConnStr"), ServerVersion.Parse("8.0.28-mysql"));
            }
        }
        public DbSet<Employee> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the table schema
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee"); // Set the table name
                //entity.HasKey(e => e.Id); // Set the primary key
                //entity.HasMany<EmployeeJobDescription>(x => x.EmployeeJobDescription);               // Configure other properties and constraints
            });

            modelBuilder.Entity<EmployeeJobDescription>(entity =>
            {
                entity.ToTable("EmployeeJobDescription");
                //entity.HasKey(e => e.Id);
                //entity.i

            });
        }
    }
}

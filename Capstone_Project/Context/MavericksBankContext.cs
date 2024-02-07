using System;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Context
{
    public class MavericksBankContext : DbContext
    {
        public DbSet<Validation> Validation { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<BankEmployees> BankEmployees { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Banks> Banks { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Beneficiaries> Beneficiaries { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Loans> Loans { get; set; }

        public MavericksBankContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Accounts entity
            modelBuilder.Entity<Accounts>()
                .Property(a => a.AccountNumber)
                .ValueGeneratedOnAdd(); // This indicates that the value is generated on add

            modelBuilder.Entity<Transactions>()
               .HasOne(t => t.Accounts)
               .WithMany(a => a.SourceTransaction)
               .HasForeignKey(t => t.SourceAccountNumber)
               .IsRequired(false);
            // Other configurations...
        }
    }
}


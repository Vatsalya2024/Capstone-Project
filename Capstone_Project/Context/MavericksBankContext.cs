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
        
            public DbSet<AvailableLoans> AvailableLoans{ get; set; }
        public MavericksBankContext(DbContextOptions options) : base(options)
        {
            Validation = Set<Validation>();
            Customers = Set<Customers>();
            BankEmployees = Set<BankEmployees>();
            Admin = Set<Admin>();
            Banks = Set<Banks>();
            Branches = Set<Branches>();
            Accounts = Set<Accounts>();
            Beneficiaries = Set<Beneficiaries>();
            Transactions = Set<Transactions>();
            Loans = Set<Loans>();
            AvailableLoans = Set<AvailableLoans>();



        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Accounts>()
                .Property(a => a.AccountNumber)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Transactions>()
               .HasOne(t => t.Accounts)
               .WithMany(a => a.SourceTransaction)
               .HasForeignKey(t => t.SourceAccountNumber)
               .IsRequired(false);
           
        }
    }
}


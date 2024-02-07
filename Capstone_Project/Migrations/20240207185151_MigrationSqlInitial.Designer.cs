﻿// <auto-generated />
using System;
using Capstone_Project.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Capstone_Project.Migrations
{
    [DbContext(typeof(MavericksBankContext))]
    [Migration("20240207185151_MigrationSqlInitial")]
    partial class MigrationSqlInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Capstone_Project.Models.Accounts", b =>
                {
                    b.Property<long>("AccountNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AccountNumber"), 1L, 1);

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.HasIndex("IFSC");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Capstone_Project.Models.Admin", b =>
                {
                    b.Property<int>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminID");

                    b.HasIndex("Email");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Capstone_Project.Models.BankEmployees", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeID");

                    b.HasIndex("Email");

                    b.ToTable("BankEmployees");
                });

            modelBuilder.Entity("Capstone_Project.Models.Banks", b =>
                {
                    b.Property<int>("BankID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankID"), 1L, 1);

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BankID");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("Capstone_Project.Models.Beneficiaries", b =>
                {
                    b.Property<long>("AccountNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("AccountNumber"), 1L, 1);

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.HasIndex("IFSC");

                    b.ToTable("Beneficiaries");
                });

            modelBuilder.Entity("Capstone_Project.Models.Branches", b =>
                {
                    b.Property<string>("IFSCNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BankID")
                        .HasColumnType("int");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IFSCNumber");

                    b.HasIndex("BankID");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Capstone_Project.Models.Customers", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"), 1L, 1);

                    b.Property<long?>("AadharNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PANNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.HasKey("CustomerID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Capstone_Project.Models.Loans", b =>
                {
                    b.Property<int>("LoanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoanID"), 1L, 1);

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<double>("Interest")
                        .HasColumnType("float");

                    b.Property<double>("LoanAmount")
                        .HasColumnType("float");

                    b.Property<string>("LoanType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Purpose")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tenure")
                        .HasColumnType("int");

                    b.HasKey("LoanID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("Capstone_Project.Models.Transactions", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("DestinationAccountNumber")
                        .HasColumnType("bigint");

                    b.Property<long>("SourceAccountNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("DestinationAccountNumber");

                    b.HasIndex("SourceAccountNumber");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Capstone_Project.Models.Validation", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Validation");
                });

            modelBuilder.Entity("Capstone_Project.Models.Accounts", b =>
                {
                    b.HasOne("Capstone_Project.Models.Customers", "Customers")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone_Project.Models.Branches", "Branches")
                        .WithMany("Accounts")
                        .HasForeignKey("IFSC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branches");

                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Capstone_Project.Models.Admin", b =>
                {
                    b.HasOne("Capstone_Project.Models.Validation", "Validation")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Validation");
                });

            modelBuilder.Entity("Capstone_Project.Models.BankEmployees", b =>
                {
                    b.HasOne("Capstone_Project.Models.Validation", "Validation")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Validation");
                });

            modelBuilder.Entity("Capstone_Project.Models.Beneficiaries", b =>
                {
                    b.HasOne("Capstone_Project.Models.Customers", "Customers")
                        .WithMany("Beneficiaries")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Capstone_Project.Models.Branches", "Branches")
                        .WithMany()
                        .HasForeignKey("IFSC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branches");

                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Capstone_Project.Models.Branches", b =>
                {
                    b.HasOne("Capstone_Project.Models.Banks", "Banks")
                        .WithMany("Branches")
                        .HasForeignKey("BankID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Banks");
                });

            modelBuilder.Entity("Capstone_Project.Models.Customers", b =>
                {
                    b.HasOne("Capstone_Project.Models.Validation", "Validation")
                        .WithOne("Customers")
                        .HasForeignKey("Capstone_Project.Models.Customers", "Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Validation");
                });

            modelBuilder.Entity("Capstone_Project.Models.Loans", b =>
                {
                    b.HasOne("Capstone_Project.Models.Customers", "Customers")
                        .WithMany("Loans")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Capstone_Project.Models.Transactions", b =>
                {
                    b.HasOne("Capstone_Project.Models.Accounts", "DestinationAccount")
                        .WithMany("Transfers")
                        .HasForeignKey("DestinationAccountNumber");

                    b.HasOne("Capstone_Project.Models.Accounts", "Accounts")
                        .WithMany("SourceTransaction")
                        .HasForeignKey("SourceAccountNumber");

                    b.Navigation("Accounts");

                    b.Navigation("DestinationAccount");
                });

            modelBuilder.Entity("Capstone_Project.Models.Accounts", b =>
                {
                    b.Navigation("SourceTransaction");

                    b.Navigation("Transfers");
                });

            modelBuilder.Entity("Capstone_Project.Models.Banks", b =>
                {
                    b.Navigation("Branches");
                });

            modelBuilder.Entity("Capstone_Project.Models.Branches", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Capstone_Project.Models.Customers", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Beneficiaries");

                    b.Navigation("Loans");
                });

            modelBuilder.Entity("Capstone_Project.Models.Validation", b =>
                {
                    b.Navigation("Customers")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

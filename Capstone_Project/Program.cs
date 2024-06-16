using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Mappers;
using Capstone_Project.Models;
using Capstone_Project.Repositories;
using Capstone_Project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Capstone_Project;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opts.JsonSerializerOptions.WriteIndented = true;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
        });
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactPolicy", opts =>
            {
                opts.WithOrigins("http://localhost:3000", "null").AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
            });
        });


        builder.Services.AddDbContext<MavericksBankContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnectionString"));
        });
        #region Repository
        builder.Services.AddScoped<IRepository<string, Validation>, ValidationRepository>();
        builder.Services.AddScoped<IRepository<int, Customers>, CustomersRepository>();
        builder.Services.AddScoped<IRepository<int, BankEmployees>, BankEmployeesRepository>();
        builder.Services.AddScoped<IRepository<int, Admin>, AdminRepository>();
        builder.Services.AddScoped<IRepository<int, Banks>, BanksRepository>();
        builder.Services.AddScoped<IRepository<string, Branches>, BranchesRepository>();
        builder.Services.AddScoped<IRepository<long, Accounts>, AccountsRepository>();
        builder.Services.AddScoped<IRepository<int, Beneficiaries>, BeneficiariesRepository>();
        builder.Services.AddScoped<IRepository<int, Loans>, LoansRepository>();
        builder.Services.AddScoped<IRepository<int, Transactions>, TransactionsRepository>();
        builder.Services.AddScoped<IRepository<int,AvailableLoans>,AvailableLoansRepository>();
        #endregion

        #region Services
        builder.Services.AddScoped<IAdminAvailableLoansService, AdminAvailableLoansService>();
        builder.Services.AddScoped<IAvailableLoansUserService, AvailableLoansUserService>();
        builder.Services.AddScoped<IBanksAdminService, BanksService>();
        builder.Services.AddScoped<IBranchesAdminService, BranchesService>();
        builder.Services.AddScoped<IBankEmployeeService, BankEmployeeService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<ICustomerLoginService, CustomerServices>();
        builder.Services.AddScoped<ICustomerAdminService, CustomerServices>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IAdminLoginService, AdminLoginService>();
        builder.Services.AddScoped<IAdministratorBankEmployeeManagementService, AdministratorBankEmployeeManagementService>();
        builder.Services.AddScoped<IBankEmployeeLoginService, BankEmployeeLoginServices>();
        builder.Services.AddScoped<IAdministratorCustomerManagementService, AdministratorCustomerManagementService>();
        builder.Services.AddScoped<IBankEmployeeTransactionService, BankEmployeeTransactionService>();
        builder.Services.AddScoped<IBankEmployeeAccountService, BankEmployeeAccountService>();
        builder.Services.AddScoped<IBankEmployeeLoanService, BankEmployeeLoanService>();
        builder.Services.AddScoped<ILoanCustomerService, CustomerLoanService> ();
        builder.Services.AddScoped<ITransactionService, CustomerTransactionService>();
        builder.Services.AddScoped<IAccountManagementService, CustomerAccountService>();
        builder.Services.AddScoped<ICustomerBeneficiaryService, CustomerBeneficiaryService>();
       
        var app = builder.Build();
        #endregion



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("ReactPolicy");
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

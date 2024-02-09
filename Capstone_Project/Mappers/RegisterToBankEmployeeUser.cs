using System;
using System.Security.Cryptography;
using System.Text;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public class RegisterToBankEmployeeUser
	{
        Validation validation;

        public RegisterToBankEmployeeUser(RegisterBankEmployeeDTO employee)
        {
            validation = new Validation();
            validation.Email = employee.Email;
            validation.UserType = "BankEmployee";
            validation.Status = "Active";
            GetPassword(employee.Password);
        }

        private void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            validation.Key = hmac.Key;
            validation.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public Validation GetValidation()
        {
            return validation;
        }
    }
}


using System;
using Capstone_Project.Models.DTOs;
using System.Security.Cryptography;
using System.Text;
using Capstone_Project.Models;

namespace Capstone_Project.Mappers
{
    public class RegisterToCustomerUser
    {
        Validation validation;
        public RegisterToCustomerUser(RegisterCustomerDTO register)
        {
            validation = new Validation();
            validation.Email = register.Email;
            validation.UserType = register.UserType;
            validation.Status = "Active";
            GetPassword(register.Password);
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


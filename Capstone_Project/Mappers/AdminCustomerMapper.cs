using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public static class AdminCustomerMapper
	{
        public static Customers MapToCustomer(AdminUpdateCustomerNameDTO nameDTO, Customers customer)
        {
            if (nameDTO.Name != null)
            {
                customer.Name = nameDTO.Name;
            }
            return customer;
        }

        public static Customers MapToCustomer(AdminUpdateCustomerContactDTO contactDTO, Customers customer)
        {
            if (contactDTO.PhoneNumber != null)
            {
                customer.PhoneNumber = contactDTO.PhoneNumber.Value;
            }
            if (contactDTO.Address != null)
            {
                customer.Address = contactDTO.Address;
            }
            if (contactDTO.AadharNumber != null)
            {
                customer.AadharNumber = contactDTO.AadharNumber;
            }
            return customer;
        }

        public static Customers MapToCustomer(AdminUpdateCustomerDetailsDTO detailsDTO, Customers customer)
        {
            if (detailsDTO.DOB != null)
            {
                customer.DOB = detailsDTO.DOB.Value;
            }
            if (detailsDTO.Age != null)
            {
                customer.Age = detailsDTO.Age.Value;
            }
            if (detailsDTO.PANNumber != null)
            {
                customer.PANNumber = detailsDTO.PANNumber;
            }
            if (detailsDTO.Gender != null)
            {
                customer.Gender = detailsDTO.Gender;
            }
            return customer;
        }
    }
}


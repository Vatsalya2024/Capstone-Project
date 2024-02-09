using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public class RegiterToBankEmployee
	{
		BankEmployees bankEmployees;
		public RegiterToBankEmployee(RegisterBankEmployeeDTO register)
		{
			bankEmployees = new BankEmployees();
			bankEmployees.Name = register.Name;
			bankEmployees.Email = register.Email;
			bankEmployees.Position = register.Position;
			bankEmployees.Phone = register.Phone;
		}
		public BankEmployees GetBankEmployees()
		{
			return bankEmployees;
		}
	}
}


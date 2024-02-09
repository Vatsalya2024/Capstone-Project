using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public class UpdateBankEmployeeByAdminMapper
	{
        public static void MapToBankEmployee(UpdateBankEmployeeByAdminDTO dto, BankEmployees bankEmployee)
        {
            if (dto == null || bankEmployee == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(dto.Position))
            {
                bankEmployee.Position = dto.Position;
            }


            if (!string.IsNullOrEmpty(dto.Phone))
            {
                bankEmployee.Phone = dto.Phone;
            }
        }
    }
}


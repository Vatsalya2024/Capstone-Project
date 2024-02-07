using System;
namespace Capstone_Project.Models.DTOs
{
    public class TransferDTO
    {

        public long SourceAccountNumber { get; set; }
        public long DestinationAccountNumber { get; set; }
        public double Amount { get; set; }

    }
}


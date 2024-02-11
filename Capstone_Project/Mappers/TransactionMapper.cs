using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
	public class TransactionMapper
	{
        public Transactions MapDepositDtoToTransaction(DepositDTO depositDTO)
        {
            return new Transactions
            {
                Amount = depositDTO.Amount,
                Description = "Deposit",
                TransactionType = "Credit",
                Status = "Completed",
                SourceAccountNumber = depositDTO.AccountNumber,
            };
        }

        public Transactions MapWithdrawalDtoToTransaction(WithdrawalDTO withdrawalDTO)
        {
            return new Transactions
            {
                Amount = withdrawalDTO.Amount,
                Description = "Withdrawal",
                TransactionType = "Debit",
                Status = "Completed",
                SourceAccountNumber = withdrawalDTO.AccountNumber,
            };
        }

        public Transactions MapTransferDtoToSourceTransaction(TransferDTO transferDTO)
        {
            return new Transactions
            {
                Amount = transferDTO.Amount,
                Description = "Transfer to " + transferDTO.DestinationAccountNumber,
                TransactionType = "Debit",
                Status = "Completed",
                SourceAccountNumber = transferDTO.SourceAccountNumber,
                DestinationAccountNumber = transferDTO.DestinationAccountNumber
            };
        }

        public Transactions MapTransferDtoToDestinationTransaction(TransferDTO transferDTO)
        {
            return new Transactions
            {
                Amount = transferDTO.Amount,
                Description = "Transfer from " + transferDTO.SourceAccountNumber,
                TransactionType = "Credit",
                Status = "Completed",
                SourceAccountNumber = transferDTO.SourceAccountNumber,
                DestinationAccountNumber = transferDTO.DestinationAccountNumber
            };
        }
    }
}




using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
    public class TransactionMapper
    {
        private Transactions _transaction;

        
        public TransactionMapper(DepositDTO depositDTO)
        {
            _transaction = new Transactions
            {
                Amount = depositDTO.Amount,
                Description = "Deposit",
                TransactionType = "Credit",
                Status = "Completed",
                SourceAccountNumber = depositDTO.AccountNumber
            };
        }

        
        public TransactionMapper(WithdrawalDTO withdrawalDTO)
        {
            _transaction = new Transactions
            {
                Amount = withdrawalDTO.Amount,
                Description = "Withdrawal",
                TransactionType = "Debit",
                Status = "Completed",
                SourceAccountNumber = withdrawalDTO.AccountNumber
            };
        }

        


        public TransactionMapper(TransferDTO transferDTO, bool isTransferFrom)
        {
            _transaction = new Transactions
            {
                Amount = transferDTO.Amount,
                Description = isTransferFrom ? $"Transfer from {transferDTO.SourceAccountNumber} to {transferDTO.DestinationAccountNumber}" : $"Transfer to {transferDTO.DestinationAccountNumber} from {transferDTO.SourceAccountNumber}",
                TransactionType = isTransferFrom ? "Debit" : "Credit",
                Status = "Completed",
                SourceAccountNumber = isTransferFrom ? transferDTO.SourceAccountNumber : transferDTO.DestinationAccountNumber,
                DestinationAccountNumber = isTransferFrom ? transferDTO.DestinationAccountNumber : transferDTO.SourceAccountNumber
            };
        }


        public Transactions GetTransaction()
        {
            return _transaction;
        }
    }
}



//using System;
//using Capstone_Project.Models;
//using Capstone_Project.Models.DTOs;

//namespace Capstone_Project.Mappers
//{
//	public class TransactionMapper
//	{
//        public Transactions MapDepositDtoToTransaction(DepositDTO depositDTO)
//        {
//            return new Transactions
//            {
//                Amount = depositDTO.Amount,
//                Description = "Deposit",
//                TransactionType = "Credit",
//                Status = "Completed",
//                SourceAccountNumber = depositDTO.AccountNumber,
//            };
//        }

//        public Transactions MapWithdrawalDtoToTransaction(WithdrawalDTO withdrawalDTO)
//        {
//            return new Transactions
//            {
//                Amount = withdrawalDTO.Amount,
//                Description = "Withdrawal",
//                TransactionType = "Debit",
//                Status = "Completed",
//                SourceAccountNumber = withdrawalDTO.AccountNumber,
//            };
//        }

//        public Transactions MapTransferDtoToSourceTransaction(TransferDTO transferDTO)
//        {
//            return new Transactions
//            {
//                Amount = transferDTO.Amount,
//                Description = "Transfer to " + transferDTO.DestinationAccountNumber,
//                TransactionType = "Debit",
//                Status = "Completed",
//                SourceAccountNumber = transferDTO.SourceAccountNumber,
//                DestinationAccountNumber = transferDTO.DestinationAccountNumber
//            };
//        }

//        public Transactions MapTransferDtoToDestinationTransaction(TransferDTO transferDTO)
//        {
//            return new Transactions
//            {
//                Amount = transferDTO.Amount,
//                Description = "Transfer from " + transferDTO.SourceAccountNumber,
//                TransactionType = "Credit",
//                Status = "Completed",
//                SourceAccountNumber = transferDTO.SourceAccountNumber,
//                DestinationAccountNumber = transferDTO.DestinationAccountNumber
//            };
//        }
//    }
//}
using System;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Mappers
{
    public class TransactionMapper
    {
        private Transactions _transaction;

        // Constructor for DepositDTO
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

        // Constructor for WithdrawalDTO
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

        // Constructor for TransferDTO
        //public TransactionMapper(TransferDTO transferDTO)
        //{
        //    _transaction = new Transactions
        //    {
        //        Amount = transferDTO.Amount,
        //        Description = "Transfer to " + transferDTO.DestinationAccountNumber,
        //        TransactionType = "Debit",
        //        Status = "Completed",
        //        SourceAccountNumber = transferDTO.SourceAccountNumber,
        //        DestinationAccountNumber = transferDTO.DestinationAccountNumber
        //    };
        //}

        //// Constructor for TransferDTO with description "Transfer from + SourceAccountNumber"
        //public TransactionMapper(TransferDTO transferDTO, bool isTransferFrom)
        //{
        //    _transaction = new Transactions
        //    {
        //        Amount = transferDTO.Amount,
        //        Description = isTransferFrom ? "Transfer from " + transferDTO.SourceAccountNumber : "Transfer to " + transferDTO.DestinationAccountNumber,
        //        TransactionType = isTransferFrom ? "Credit" : "Debit",
        //        Status = "Completed",
        //        SourceAccountNumber = isTransferFrom ? transferDTO.SourceAccountNumber : transferDTO.DestinationAccountNumber,
        //        DestinationAccountNumber = isTransferFrom ? transferDTO.DestinationAccountNumber : transferDTO.SourceAccountNumber
        //    };
        //}


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



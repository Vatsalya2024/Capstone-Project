using System;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Capstone_Project.Models.DTOs;

namespace Capstone_Project.Services
{
    public class BanksService : IBanksAdminService
    {
        private readonly IRepository<int, Banks> _banksRepository;
        private readonly ILogger<BanksService> _loggerBanksService;

        public BanksService(IRepository<int, Banks> banksRepository, ILogger<BanksService> loggerBanksService)
        {
            _banksRepository = banksRepository;
            _loggerBanksService = loggerBanksService;
        }

        public async Task<Banks> AddBank(Banks item)
        {
            _loggerBanksService.LogInformation("Adding bank...");
            return await _banksRepository.Add(item);
        }

        public async Task<Banks> DeleteBank(int key)
        {
            var deletedBank = await _banksRepository.Delete(key);
            if (deletedBank == null)
            {
                throw new NoBanksFoundException($"Bank ID {key} not found");
            }
            _loggerBanksService.LogInformation("Bank Deleted");
            return deletedBank;
        }

        public async Task<List<Banks>> GetAllBanks()
        {
            var allBanks = await _banksRepository.GetAll();
            if (allBanks == null)
            {
                throw new NoBanksFoundException($"No Banks Data Found");
            }
            _loggerBanksService.LogInformation("Banks Fetched");
            return allBanks;
        }

        public async Task<Banks> GetBank(int key)
        {
            var foundedBank = await _banksRepository.Get(key);
            if (foundedBank == null)
            {
                throw new NoBanksFoundException($"Bank ID {key} not found");
            }
            _loggerBanksService.LogInformation("Bank Found");
            return foundedBank;
        }

        public async Task<Banks> UpdateBankName(UpdateBankNameDTO updateBankNameDTO)
        {
            var foundedBank = await GetBank(updateBankNameDTO.BankID);
            foundedBank.BankName = updateBankNameDTO.BankName;
            var updatedBank = await _banksRepository.Update(foundedBank);
            _loggerBanksService.LogInformation("Bank Updated");
            return updatedBank;
        }
    }
}


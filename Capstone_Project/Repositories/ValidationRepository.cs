using System;
using Capstone_Project.Context;
using Capstone_Project.Interfaces;
using Capstone_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Project.Repositories
{
    public class ValidationRepository : IRepository<string, Validation>
    {
        private readonly MavericksBankContext _mavericksBankContext;
        private readonly ILogger<ValidationRepository> _loggerValidationRepository;
        

        public ValidationRepository(MavericksBankContext mavericksBankContext, ILogger<ValidationRepository> loggerValidationRepository)
        {
            _mavericksBankContext = mavericksBankContext;
            _loggerValidationRepository = loggerValidationRepository;
        }

        public async Task<Validation> Add(Validation item)
        {
            _mavericksBankContext.Validation.Add(item);
            await _mavericksBankContext.SaveChangesAsync();
            _loggerValidationRepository.LogInformation($"Added New Validation : {item.Email}");
            return item;
        }

        public async Task<Validation?> Delete(string key)
        {
            var foundedValidation = await Get(key);
            if (foundedValidation == null)
            {
                return null;
            }
            else
            {
                _mavericksBankContext.Validation.Remove(foundedValidation);
                await _mavericksBankContext.SaveChangesAsync();
                return foundedValidation;
            }
        }

        public async Task<Validation?> Get(string key)
        {
            var foundedValidation = await _mavericksBankContext.Validation.FirstOrDefaultAsync(validation => validation.Email == key);
            if (foundedValidation == null)
            {
                return null;
            }
            else
            {
                return foundedValidation;
            }
        }

        public async Task<List<Validation>?> GetAll()
        {
            var allValidations = await _mavericksBankContext.Validation.ToListAsync();
            if (allValidations.Count == 0)
            {
                return null;
            }
            else
            {
                return allValidations;
            }
        }

        public async Task<Validation> Update(Validation item)
        {
            _mavericksBankContext.Entry<Validation>(item).State = EntityState.Modified;
            await _mavericksBankContext.SaveChangesAsync();
            return item;
        }
    }
}


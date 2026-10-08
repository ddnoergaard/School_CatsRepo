using CatsRepo.DTO.Cat;
using CatsRepo.Models;
using CatsRepo.Repositories.Interfaces;
using CatsRepo.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace CatsRepo.Services
{
    public class CatsService : ICatsService
    {
        private readonly ICatsRepository _catsRepository;

        public CatsService(ICatsRepository catsRepository)
        {
            _catsRepository = catsRepository;
        }

        public async Task<Cat?> CreateAsync(CreateDTO dto)
        {
            try
            {
                int newId = await _catsRepository.CreateAsync(dto);
                return await _catsRepository.GetCatByIdAsync(newId);
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public async Task<Cat> GetById(int id)
        {
            Cat? cat = await _catsRepository.GetCatByIdAsync(id);

            return cat is null ? throw new KeyNotFoundException("No cat found") : cat;
        }

        public async Task<IEnumerable<Cat>> Get(string? name = null, int? minWeight = null)
        {
            IEnumerable<Cat> cats = await _catsRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(name))
            {
                cats = cats.Where(c => c.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase));
            }
            if (minWeight != 0 || minWeight is not null) cats = cats.Where(c => c.Weight >= minWeight);
            return cats;
        }

    }
}

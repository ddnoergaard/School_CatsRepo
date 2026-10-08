using CatsRepo.DTO.Cat;
using CatsRepo.Models;

namespace CatsRepo.Services.Interfaces
{
    public interface ICatsService
    {
        Task<Cat?> CreateAsync(CreateDTO dto);
        Task<Cat> GetById(int id);
        Task<IEnumerable<Cat>> Get(string? substring = null, int? minWeight = null);
    }
}
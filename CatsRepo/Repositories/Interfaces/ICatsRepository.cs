using CatsRepo.DTO.Cat;
using CatsRepo.Models;

namespace CatsRepo.Repositories.Interfaces
{
    public interface ICatsRepository
    {
        Task<int> CreateAsync(CreateDTO dto);
        Task Delete(int id);
        Task<IEnumerable<Cat>> GetAllAsync();
        Task<Cat?> GetCatByIdAsync(int id);
        Task UpdateAsync(int id, Cat cat);
    }
}
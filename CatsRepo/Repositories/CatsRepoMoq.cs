using CatsRepo.DTO.Cat;
using CatsRepo.Models;
using CatsRepo.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CatsRepo.Repositories
{
    public class CatsRepoMoq : ICatsRepository
    {
        private List<Cat> _cats;
        private int nextId;

        public CatsRepoMoq()
        {
            _cats = new List<Cat>
            {
                new Cat{Id = 1, Name = "One", Weight = 1.1},
                new Cat{Id = 2, Name = "Two", Weight = 1.3},
                new Cat{Id = 3, Name = "Three", Weight = 3.4},
                new Cat{Id = 4, Name = "Four", Weight = 4.3}
            };
        }

        public async Task<int> CreateAsync(CreateDTO dto)
        {
            _cats.Add(new Cat
            {
                Id = nextId++,
                Name = dto.Name,
                Weight = dto.Weight
            });

            return nextId;
        }

        public async Task<IEnumerable<Cat>> GetAllAsync()
        {
            return new List<Cat>(_cats);
        }

        public async Task<Cat?> GetCatByIdAsync(int id)
        {
            return _cats.FirstOrDefault(c => c.Id == id);
        }

        public async Task UpdateAsync(int id, Cat cat)
        {
            foreach (Cat c in _cats)
            {
                if (c.Id == id)
                {
                    c.Id = cat.Id;
                    c.Name = cat.Name;
                    c.Weight = cat.Weight;
                }
            }
        }

        public async Task Delete(int id)
        {
            Cat catToRemove = _cats.FirstOrDefault(c => c.Id == id);
            _cats.Remove(catToRemove);
        }
    }
}

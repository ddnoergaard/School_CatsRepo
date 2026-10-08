using CatsRepo.DTO.Cat;
using CatsRepo.Models;
using CatsRepo.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;

namespace CatsRepo.Repositories
{
    public class CatsRepository : ICatsRepository
    {
        private string _connectionString;


        public CatsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateAsync(CreateDTO dto)
        {
            string sqlStatement = "INSERT INTO Cats(name, weight) OUTPUT Inserted.id VALUES(@name, @weight)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    cmd.Parameters.AddWithValue("@name", dto.Name);
                    cmd.Parameters.AddWithValue("@weight", dto.Weight);

                    int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    return newId;

                    //using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    //{
                    //    if (await reader.ReadAsync())
                    //    {
                    //        return new Cat
                    //        {
                    //            Id = Convert.ToInt32(reader["id"]),
                    //            Name = Convert.ToString(reader["name"]),
                    //            Weight = Convert.ToDouble(reader["weight"])
                    //        };
                    //    }
                    //    return null;
                    //}
                }
            }
        }

        public async Task<IEnumerable<Cat>> GetAllAsync()
        {
            string sqlQuery = "SELECT * FROM Cats";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        List<Cat> cats = new();
                        while (await reader.ReadAsync())
                        {
                            cats.Add(new Cat
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = Convert.ToString(reader["name"]),
                                Weight = Convert.ToDouble(reader["weight"])
                            });
                        }
                        return cats;
                    }
                }
            }
        }

        public async Task<Cat?> GetCatByIdAsync(int id)
        {
            string sqlQuery = "SELECT * FROM Cats WHERE id = @id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Cat
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = Convert.ToString(reader["name"]),
                                Weight = Convert.ToDouble(reader["weight"])
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task UpdateAsync(int id, Cat cat)
        {
            string sqlStatement = "UPDATE Cats " +
                "SET name = @name, weight = @weight " +
                "WHERE id = @id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", cat.Name);
                    cmd.Parameters.AddWithValue("@weight", cat.Weight);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(int id)
        {
            string sqlStatement = "DELETE FROM Cats WHERE id = @id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

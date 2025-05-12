using System.Data.Common;
using apbd_test1.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace apbd_test1.Services;

public class DbService : IDbService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

    public async Task<VisitDTO> GetVisit(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = connection.CreateCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = connection.BeginTransaction();
        command.Transaction = transaction as SqlTransaction;

        VisitDTO visit;
        
        try
        {
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Visits WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            
            int client_id = -1;
            int mechanic_id = -1;
            
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    int visitIdOrdinal = reader.GetOrdinal("visit_id");
                    int clientIdOrdinal = reader.GetOrdinal("client_id");
                    int mechanicIdOrdinal = reader.GetOrdinal("mechanic_id");
                    int dateOrdinal = reader.GetOrdinal("date");

                    client_id = reader.GetOrdinal("client_id");
                    mechanic_id = reader.GetOrdinal("mechanic_id");
                    
                    visit = new VisitDTO()
                    {
                        Date = reader.GetDateTime(dateOrdinal),
                    };
                }else throw new FileNotFoundException("Could not find visit with given ID");
            }
            
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Client WHERE client_id = @client_id";
            command.Parameters.AddWithValue("@client_id", client_id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    int clientIdOrdinal = reader.GetOrdinal("client_id");
                    int firstNameOrdinal = reader.GetOrdinal("first_name");
                    int lastNameOrdinal = reader.GetOrdinal("last_name");
                    int DateOfBirthOrdinal = reader.GetOrdinal("date_of_birth");

                    visit.Client = new ClientDTO()
                    {
                        FirstName = reader.GetString(firstNameOrdinal),
                        LastName = reader.GetString(lastNameOrdinal),
                        DateOfBirth = reader.GetDateTime(DateOfBirthOrdinal),
                    };
                }
            }
            
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Mechanic WHERE mechanic_id = @mechanic_id";
            command.Parameters.AddWithValue("@mechanic_id", mechanic_id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return visit;
    }
}
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
            command.CommandText = "SELECT * FROM Visit WHERE visit_id = @id";
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
                        ServiceVisits = new List<ServiceVisitDTO>()
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
                if (await reader.ReadAsync())
                {
                    int mechanicIdOrdinal = reader.GetOrdinal("mechanic_id");
                    int firstNameOrdinal = reader.GetOrdinal("first_name");
                    int lastNameOrdinal = reader.GetOrdinal("last_name");
                    int licenceNumberOrdinal = reader.GetOrdinal("licence_number");

                    visit.Mechanic = new MechanicDTO()
                    {
                        MechanicId = reader.GetInt32(mechanicIdOrdinal),
                        LicenceNumber = reader.GetString(licenceNumberOrdinal),
                    };
                }
            }
            
            command.Parameters.Clear();
            command.CommandText = "SELECT * FROM Service INNER JOIN Visit_Service ON Service.service_id = Visit_Service.service_id WHERE Visit_Service.visit_id = @visit_id";
            command.Parameters.AddWithValue("@visit_id", id);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int nameOrdinal = reader.GetOrdinal("name");
                    int serviceFeeOrdinal = reader.GetOrdinal("service_fee");

                    visit.ServiceVisits.Add(new ServiceVisitDTO()
                    {
                        Name = reader.GetString(nameOrdinal),
                        ServiceFee = reader.GetDecimal(serviceFeeOrdinal),
                    });
                }
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
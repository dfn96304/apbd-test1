using apbd_test1.Models.DTOs;

namespace apbd_test1.Services;

public interface IDbService
{
    public Task<VisitDTO> GetVisit(int id);
    public Task NewVisit(CreateVisitDTO visit);
}
namespace apbd_test1.Models.DTOs;

public class CreateVisitDTO
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string MechanicLicenceNumber { get; set; }
    public List<CreateServiceDTO> Services { get; set; }
}
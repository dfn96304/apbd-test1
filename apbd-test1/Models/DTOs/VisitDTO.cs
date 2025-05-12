namespace apbd_test1.Models.DTOs;

public class VisitDTO
{
    public DateTime Date { get; set; }
    public ClientDTO Client { get; set; }
    public MechanicDTO Mechanic { get; set; }
    public VisitServiceDTO VisitService { get; set; }
}
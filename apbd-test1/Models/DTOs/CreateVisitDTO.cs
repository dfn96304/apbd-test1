using System.ComponentModel.DataAnnotations;

namespace apbd_test1.Models.DTOs;

public class CreateVisitDTO
{
    [Required]
    public int VisitId { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    [MaxLength(14)]
    public string MechanicLicenceNumber { get; set; }
    public List<CreateServiceDTO> Services { get; set; }
}
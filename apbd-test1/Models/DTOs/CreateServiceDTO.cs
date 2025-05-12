using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace apbd_test1.Models.DTOs;

public class CreateServiceDTO
{
    [Required]
    [MaxLength(100)]
    public string ServiceName { get; set; }
    [Required]
    public decimal ServiceFee { get; set; }
}
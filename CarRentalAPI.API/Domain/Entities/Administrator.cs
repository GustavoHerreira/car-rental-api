using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRentalAPI.Domain.Enums;

namespace CarRentalAPI.Domain.Entities;

public class Administrator
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(32)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [StringLength(16)]
    public AdminRoleEnum Role { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Dto;

public class RoleDto
{
    public int Id { get; set; }
    [Required]
    public string RoleName { get; set; }
    public string Description { get; set; }
    [Required]
    public bool IsActive { get; set; }
}
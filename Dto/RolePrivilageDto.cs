using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Dto;

public class RolePrivilageDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string RolePrivilageName { get; set; }
    public string Description { get; set; }
    [Required]
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
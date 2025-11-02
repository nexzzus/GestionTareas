using System.ComponentModel.DataAnnotations;

namespace GestionTareas.Web.Data.Entities;

public class TaskEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(32)]
    public required string Title { get; set; }
    
    [MaxLength(264)]
    public string? Description { get; set; }
    
    public DateTime Created { get; set; } =  DateTime.Now;
    
    [Required]
    public required DateTime Finished { get; set; }

    public bool Status { get; set; } = false;
}
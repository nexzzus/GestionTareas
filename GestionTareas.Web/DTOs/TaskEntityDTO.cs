using System.ComponentModel.DataAnnotations;

namespace GestionTareas.Web.DTOs;

public class TaskEntityDTO
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "El campo '{0}' es obligatorio.")]
    [MaxLength(32)]
    [Display(Name = "Título")]
    public required string Title { get; set; }
    
    [MaxLength(264)]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "El campo '{0}' es obligatorio.")]
    [Display(Name = "Fecha de finalización")]
    public DateTime Finished { get; set; }
    
    [Display(Name = "Estado")]
    public bool Status { get; set; }
}
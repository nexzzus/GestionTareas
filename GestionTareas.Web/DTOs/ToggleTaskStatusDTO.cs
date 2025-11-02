using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.Web.DTOs;

public class ToggleTaskStatusDTO
{
    [Required(ErrorMessage = "El campo es requerido.")]
    public Guid TaskId { get; set; }
    
    public bool Status { get; set; } = false;
}
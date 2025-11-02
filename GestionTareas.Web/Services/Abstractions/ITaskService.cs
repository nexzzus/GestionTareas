using GestionTareas.Web.Core;
using GestionTareas.Web.DTOs;

namespace GestionTareas.Web.Services.Abstractions;

public interface ITaskService
{
    public Task<Response<TaskEntityDTO>> CreateTask(TaskEntityDTO dto);
    public Task<Response<TaskEntityDTO>> UpdateTask(TaskEntityDTO dto);
    public Task<Response<object>> DeleteTask(Guid id);
    public Task<Response<List<TaskEntityDTO?>>> GetAll();
    public Task<Response<TaskEntityDTO?>> GetOne(Guid id);
    public Task<Response<object>> ToggleAsync(ToggleTaskStatusDTO dto);
    public Task<Response<List<TaskEntityDTO?>>> Filter(string? search, DateTime? startDate, DateTime? endDate, bool? status);
}
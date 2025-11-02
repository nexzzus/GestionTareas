using AutoMapper;
using GestionTareas.Web.Core;
using GestionTareas.Web.Data;
using GestionTareas.Web.Data.Entities;
using GestionTareas.Web.DTOs;
using GestionTareas.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace GestionTareas.Web.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TaskService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<TaskEntityDTO>> CreateTask(TaskEntityDTO dto)
    {
        try
        {
            TaskEntity task = _mapper.Map<TaskEntity>(dto);
            task.Id = Guid.NewGuid();
            task.Created = DateTime.UtcNow;

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            dto.Id = task.Id;

            return Response<TaskEntityDTO>.Success(_mapper.Map<TaskEntityDTO>(task), "Tarea realizada con éxito");
        }
        catch (Exception e)
        {
            return Response<TaskEntityDTO>.Failure(e.Message);
        }
    }

    public async Task<Response<TaskEntityDTO>> UpdateTask(TaskEntityDTO dto)
    {
        try
        {
            TaskEntity? task = _context.Tasks.FirstOrDefault(t => t.Id == dto.Id);
            if (task is null)
            {
                return Response<TaskEntityDTO>.Failure($"La tarea con Id '{dto.Id}' no existe.");
            }

            task = _mapper.Map(dto, task);
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Response<TaskEntityDTO>.Success(dto, "Tarea actualizada con éxito.");
        }
        catch (Exception e)
        {
            return Response<TaskEntityDTO>.Failure(e.Message);
        }
    }

    public async Task<Response<object>> DeleteTask(Guid id)
    {
        try
        {
            TaskEntity? task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task is null)
            {
                return Response<object>.Failure($"La tarea con Id '{id}' no existe.");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Response<object>.Success("Tarea eliminada.");
        }
        catch (Exception e)
        {
            return Response<object>.Failure(e.Message);
        }
    }

    public async Task<Response<List<TaskEntityDTO?>>> GetAll()
    {
        try
        {
            List<TaskEntity> tasks = await _context.Tasks.ToListAsync();
            List<TaskEntityDTO> dtos = _mapper.Map<List<TaskEntityDTO>>(tasks);

            return Response<List<TaskEntityDTO?>>.Success(dtos!);
        }
        catch (Exception e)
        {
            return Response<List<TaskEntityDTO?>>.Failure(e.Message);
        }
    }

    public async Task<Response<object>> ToggleAsync(ToggleTaskStatusDTO dto)
    {
        try
        {
            TaskEntity? task = await _context.Tasks.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == dto.TaskId);
            if (task is null)
            {
                return Response<object>.Failure($"No exite la tarea con Id '{dto.TaskId}'");
            }

            task.Status = dto.Status;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Response<object>.Success("Tarea actualizada.");
        }
        catch (Exception e)
        {
            return Response<object>.Failure(e);
        }
    }

    public async Task<Response<List<TaskEntityDTO?>>> Filter(string? search, DateTime? startDate, DateTime? endDate,
        bool? status)
    {
        IQueryable<TaskEntity> query = _context.Tasks.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(t =>
                    EF.Functions.Collate(t.Title, "SQL_Latin1_General_CP1_CI_AI").Contains(search) ||
                    EF.Functions.Collate(t.Description, "SQL_Latin1_General_CP1_CI_AI")!.Contains(search)
                );
        }

        if (startDate.HasValue)
        {
            query =  query.Where(t => t.Finished >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(t => t.Finished <= endDate.Value.Date);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        List<TaskEntityDTO> tasks = await query
            .OrderBy(t => t.Finished)
            .Select(t => new TaskEntityDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Finished = t.Finished,
                Status = t.Status,
            })
            .ToListAsync();
        return Response<List<TaskEntityDTO?>>.Success(tasks!);
    }

    public async Task<Response<TaskEntityDTO?>> GetOne(Guid id)
    {
        TaskEntity? task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null)
        {
            return Response<TaskEntityDTO?>.Failure($"No existe una tarea con Id '{id}'");
        }

        return Response<TaskEntityDTO?>.Success(_mapper.Map<TaskEntityDTO>(task));
    }
}
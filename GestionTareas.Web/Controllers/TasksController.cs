using System.Text.Json.Serialization;
using AspNetCoreHero.ToastNotification.Abstractions;
using GestionTareas.Web.Core;
using GestionTareas.Web.DTOs;
using GestionTareas.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.Web.Controllers;

public class TasksController : Controller
{
    private readonly ITaskService _taskService;
    private readonly INotyfService _notyfService;

    public TasksController(ITaskService taskService, INotyfService notyfService)
    {
        _taskService = taskService;
        _notyfService = notyfService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, DateTime? startDate, DateTime? endDate, bool? status)
    {
        Response<List<TaskEntityDTO?>> tasks = await _taskService.GetAll();

        Response<List<TaskEntityDTO?>> tasks2 = await _taskService.Filter(search, startDate, endDate, status);
        return View(tasks2);
    }


    [HttpPost]
    public async Task<IActionResult> Toggle([FromForm] ToggleTaskStatusDTO dto)
    {
        Response<object> response = await _taskService.ToggleAsync(dto);
        if (!response.isSuccess)
        {
            _notyfService.Error(response.Message);
        }
        _notyfService.Success(response.Message);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] TaskEntityDTO dto)
    {
        if (!ModelState.IsValid)
        {
            _notyfService.Error("Complete los campos requeridos.");
            return View(dto);
        }

        Response<TaskEntityDTO> response = await _taskService.CreateTask(dto);
        if (!response.isSuccess)
        {
            _notyfService.Error(response.Message);
            return View(dto);
        }
        
        _notyfService.Success(response.Message);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        Response<TaskEntityDTO?> respose = await _taskService.GetOne(id);
        if (!respose.isSuccess)
        {
            _notyfService.Error(respose.Message);
            return RedirectToAction(nameof(Index));
        }
        return View(respose.Result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] TaskEntityDTO dto)
    {
        if (!ModelState.IsValid)
        {
            _notyfService.Error("Complete los campos requeridos");
            return View(dto);
        }

        Response<TaskEntityDTO> response = await _taskService.UpdateTask(dto);
        if (!response.isSuccess)
        {
            _notyfService.Error(response.Message);
            return View(dto);
        }
        _notyfService.Success(response.Message);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Response<object> response = await _taskService.DeleteTask(id);
        if (!response.isSuccess)
        {
            _notyfService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }
        _notyfService.Success(response.Message);
        return RedirectToAction(nameof(Index));
    }
}
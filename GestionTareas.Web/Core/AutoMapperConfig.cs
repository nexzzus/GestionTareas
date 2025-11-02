using AutoMapper;
using GestionTareas.Web.Data.Entities;
using GestionTareas.Web.DTOs;

namespace GestionTareas.Web.Core;

public class AutoMapperConfig: Profile
{
    public AutoMapperConfig()
    {
        CreateMap<TaskEntity, TaskEntityDTO>().ReverseMap();
    }
}
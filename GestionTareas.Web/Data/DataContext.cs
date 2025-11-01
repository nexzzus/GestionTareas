using GestionTareas.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas.Web.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<TaskEntity>  Tasks { get; set; }
}
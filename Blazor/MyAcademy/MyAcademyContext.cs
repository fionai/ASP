using Microsoft.EntityFrameworkCore;

public class MyAcademyContext(DbContextOptions<MyAcademyContext> options) : DbContext(options)
{
    public DbSet<MyAcademy.Models.Direction> Directions { get; set; } = default!;
    public DbSet<MyAcademy.Models.Discipline> Disciplines { get; set; } = default!;
}

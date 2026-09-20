using Microsoft.EntityFrameworkCore;

public class MyAcademyContext(DbContextOptions<MyAcademyContext> options) : DbContext(options)
{
    public DbSet<MyAcademy.Models.Direction> Directions { get; set; } = default!;
    public DbSet<MyAcademy.Models.Discipline> Disciplines { get; set; } = default!;
    public DbSet<MyAcademy.Models.Group> Groups { get; set; } = default!;
    public DbSet<MyAcademy.Models.Student> Students { get; set; } = default!;
    public DbSet<MyAcademy.Models.Teacher> Teachers { get; set; } = default!;
}

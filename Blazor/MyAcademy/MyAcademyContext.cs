using Microsoft.EntityFrameworkCore;

public class MyAcademyContext(DbContextOptions<MyAcademyContext> options) : DbContext(options)
{
    public DbSet<MyAcademy.Models.Direction> Directions { get; set; } = default!;
}

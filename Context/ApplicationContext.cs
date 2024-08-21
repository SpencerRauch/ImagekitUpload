#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using ImagekitUpload.Models;

namespace ImagekitUpload.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
}

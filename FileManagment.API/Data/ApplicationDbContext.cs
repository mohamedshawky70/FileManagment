using FileManagment.API.Entites;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FileManagment.API.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}
	public DbSet<UploadFile> Files { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//أي كلاس يبانبلمنت الانرفيس (أ انتتي كونفجريشن)هيضيفه هنا بدل مكنت هضيف عشروميت كلاس لو عندي عشروميت تابل 
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}

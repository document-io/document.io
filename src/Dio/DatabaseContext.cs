using Microsoft.EntityFrameworkCore;

namespace Dio
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
			
		}
	}
}
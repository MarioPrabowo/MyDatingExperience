using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace MyDatingExperience.Repository
{
    public static class Bootstrap
    {
		public static IServiceCollection RegisterInMemoryDbServices([NotNullAttribute] this IServiceCollection serviceCollection)
		{
			if (!serviceCollection.Any(s => s.ServiceType.IsAssignableFrom(typeof(DateDbContext))))
			{
				serviceCollection.AddDbContext<DateDbContext>((IServiceProvider prov, DbContextOptionsBuilder optionsBuilder) =>
				{
					if (!optionsBuilder.IsConfigured)
					{
						optionsBuilder.UseInMemoryDatabase(databaseName: DateDbContext.DbName);
					}
				});
			}

			return serviceCollection;
		}
	}
}

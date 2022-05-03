using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MyDatingExperience.Repository
{
	public class DateDbContext : DbContext
	{
		public static readonly string DbName = "DateInMemoryDB";

		public virtual DbSet<HotelBookingEntity> HotelBooking { get; set; }
		public virtual DbSet<RestaurantReservationEntity> RestaurantReservation { get; set; }
		public virtual DbSet<InvoiceEntity> Invoice { get; set; }

		public DateDbContext(DbContextOptions<DateDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<HotelBookingEntity>(entity =>
			{
				entity.HasKey(e => e.BookingId);
			});

			modelBuilder.Entity<RestaurantReservationEntity>(entity =>
			{
				entity.HasKey(e => e.BookingId);
			});

			modelBuilder.Entity<InvoiceEntity>(entity =>
			{
				entity.HasKey(e => e.InvoiceId);
			});

			ConvertDateFieldsToUtc(modelBuilder);
		}

		private void ConvertDateFieldsToUtc(ModelBuilder builder)
		{
			var dateTimeValueConverter = new ValueConverter<DateTime, DateTime>(
						   v => v,
						   v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			var nullableDateTimeValueConverter = new ValueConverter<DateTime?, DateTime?>(
							v => v,
							v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

			foreach (var entity in builder.Model.GetEntityTypes())
			{
				foreach (var property in entity.GetProperties())
				{
					if (property.ClrType == typeof(DateTime))
					{
						property.SetValueConverter(dateTimeValueConverter);
					}

					if (property.ClrType == typeof(DateTime?))
					{
						property.SetValueConverter(nullableDateTimeValueConverter);
					}
				}
			}
		}
	}
}

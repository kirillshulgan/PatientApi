using Microsoft.EntityFrameworkCore;
using PatientApi.Models;

namespace PatientApi.Data
{
    /// <summary>
    /// Контекст базы данных для управления пациентами.
    /// </summary>
    public class PatientContext: DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса с заданными параметрами.
        /// </summary>
        /// <param name="options">Параметры контекста базы данных.</param>
        public PatientContext(DbContextOptions<PatientContext> options) : base(options) { }

        /// <summary>
        /// Представление пациентов в базе данных.
        /// </summary>
        public DbSet<Patient> Patients { get; set; }

        /// <summary>
        /// Конфигурация модели данных.
        /// </summary>
        /// <param name="modelBuilder">Объект для построения модели данных.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .Property(p => p.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Active)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .OwnsOne(p => p.Name, name =>
                {
                    name.Property(n => n.Use).HasColumnName("Use");
                    name.Property(n => n.Family).HasColumnName("Family");
                    name.Property(n => n.Given).HasColumnName("Given");
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}

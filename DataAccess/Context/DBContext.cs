using Domain;
using Domain.Alumnos;
using Domain.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccess.Context
{
    public class PilatesContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Local> Local { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Patologia> Patologias { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Plan> Planes { get; set; }
        public DbSet<ClaseFija> ClasesFijas { get; set; }
        public DbSet<Falta> Faltas { get; set; }
        public DbSet<Logs_AddAlumnoClase> Logs_AlumnoClase { get; set; }
        public PilatesContext() { }
        public PilatesContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string directory = Directory.GetCurrentDirectory();
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(directory)
                .AddJsonFile("appsettings.json")
                .Build();
                string connectionString = configuration.GetConnectionString(@"PilatesProDB");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clase>()
            .HasOne(c => c.Agenda)
            .WithOne(a => a.Clase)
            .HasForeignKey<Agenda>(a => a.ClaseId)
            .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la entidad Clase
            modelBuilder.Entity<Clase>(entity =>
            {
                entity.HasKey(e => e.Id); // Llave primaria
                entity.HasMany(e => e.ClasesAlumno)
                    .WithOne(ac => ac.Clase)
                    .HasForeignKey(ac => ac.ClaseId)
                    .OnDelete(DeleteBehavior.Cascade); // Elimina ClasesAlumno cuando se elimina Clase
            });

            // Definir que el atributo Email sea único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();  // Agregar la restricción de unicidad

            base.OnModelCreating(modelBuilder);
        }
    }
}

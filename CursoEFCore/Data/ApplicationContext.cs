using System;
using System.Linq;
using CursoEFCoreConsole.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCoreConsole.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Data source=DSDEV004MF\\SQLEXPRESS; Initial Catalog=EFCoreCurso; User Id=sa;Password=docspider; Trusted_Connection=False;",
                p => p.EnableRetryOnFailure(
                    maxRetryCount: 2, 
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null).MigrationsHistoryTable("Curso_EF_Core"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        protected void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    if(string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        // property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }

                }
            }
        }
    }
}
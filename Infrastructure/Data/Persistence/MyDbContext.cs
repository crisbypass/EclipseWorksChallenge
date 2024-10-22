using Domain.Entities;
using Infrastructure.Data.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Persistence
{
    public class MyDbContext(IConfiguration configuration,
        ILogger<MyDbContext> logger) : DbContext
    {
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<MyDbContext> _logger = logger;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

            optionsBuilder.UseInMemoryDatabase(connectionString)
                .LogTo(message => _logger.Log(LogLevel.Information, message));

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjetoConfiguration());
            modelBuilder.ApplyConfiguration(new TarefaConfiguration());
            modelBuilder.ApplyConfiguration(new HistoricoConfiguration());
            modelBuilder.ApplyConfiguration(new ComentarioConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

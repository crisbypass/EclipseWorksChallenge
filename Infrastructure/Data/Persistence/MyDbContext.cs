using Domain.Entities;
using Infrastructure.Data.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Persistence
{
    /// <summary>
    /// Em grande parte dos casos, devido ao uso da unidade de trabalho para
    /// respositórios de entidades distintas compartilhando o mesmo contexto,
    /// foi adotada a estratégia de atualização de entidades relacionadas
    /// através do fornecimento do valor da chave estrangeira, em detrimento
    /// do trabalho com as propriedades de navegação do tipo de 
    /// coleções(adicionar, remover, etc.), embora estejam em uso para
    /// contribuir na configuração do Entity Framework Core.
    /// </summary>
    public class MyDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
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

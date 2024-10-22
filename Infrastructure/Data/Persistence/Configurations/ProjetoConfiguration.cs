using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Persistence.Configurations
{
    public class ProjetoConfiguration : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(t => t.Tarefas)
             .WithOne(p => p.Projeto)
             .HasForeignKey(p => p.ProjetoId)
             .OnDelete(DeleteBehavior.NoAction);
            //builder.ToTable("MyTable"); // Defina o nome da tabela se necessário
        }
    }
}

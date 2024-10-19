using EclipseWorksChallenge.MyData.MyEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EclipseWorksChallenge.MyData.MyConfigurations
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
            //builder.ToTable("Autor"); // Defina o nome da tabela se necessário
        }
    }
}

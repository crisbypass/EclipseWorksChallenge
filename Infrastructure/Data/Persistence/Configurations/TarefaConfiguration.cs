using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Persistence.Configurations
{
    public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(t => t.Historicos)
             .WithOne(p => p.Tarefa)
             .HasForeignKey(p => p.TarefaId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.Comentarios)
             .WithOne(p => p.Tarefa)
             .HasForeignKey(p => p.TarefaId)
             .OnDelete(DeleteBehavior.NoAction);

            //builder.Property(p => p.Prioridade)
            //.HasConversion(v => v.ToString(), v => (Prioridade)Enum.Parse(typeof(Prioridade), v));
            //builder.Property(p => p.Status)
            //.HasConversion(v => v.ToString(), v => (Status)Enum.Parse(typeof(Status), v));

            builder.Property(p => p.Prioridade)
            .HasConversion(v => Convert.ToInt32(v), v => (PrioridadeEnum)v);
            builder.Property(p => p.Status)
            .HasConversion(v => Convert.ToInt32(v), v => (StatusEnum)v);
        }
    }
}

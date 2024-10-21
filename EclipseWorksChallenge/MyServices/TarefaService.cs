using EclipseWorksChallenge.MyData.MyEntities;
using EclipseWorksChallenge.MyData.MyRepositories;
using EclipseWorksChallenge.MyDtos;
using EclipseWorksChallenge.MyModels;

namespace EclipseWorksChallenge.MyServices
{
    public class TarefaService(IGenericRepository<Tarefa> tarefaRepository)
    {
        private readonly IGenericRepository<Tarefa> _tarefaRepository = tarefaRepository;
        private async Task<bool> PermiteCriarTarefaAsync(int projetoId)
        {
            var count = await _tarefaRepository.ContarItemsAsync(filtro: x => x.ProjetoId == projetoId);
            return count < 20;
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string MensagemValidacao)> CriarTarefaAsync(
            int projetoId, TarefaModel tarefaModel, string responsavel)
        {
            if (await PermiteCriarTarefaAsync(projetoId))
            {
                var tarefa = new Tarefa { };

                tarefa.Historicos.Add(new Historico
                {
                    DataAtualizacao = DateTime.Now,
                    Responsavel = responsavel,
                    Resumo = "Nova tarefa criada."
                });

                var inserted = await _tarefaRepository.InserirAsync(tarefa);

                var dto = new TarefaDto { };

                return (true, dto, null!);
            }
            return (false, default!, "O número limite de tarefas foi atingido para o projeto.");
        }
    }
}

using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class TarefaService(
        IUnityOfWork unityOfWork
        //IGenericRepository<Tarefa> tarefaRepository,
        //IGenericRepository<Historico> historicoRepository
        )
    {
        private readonly IUnityOfWork _unityOfWork = unityOfWork;

        //private readonly IGenericRepository<Tarefa> _tarefaRepository = tarefaRepository;
        //private readonly IGenericRepository<Historico> _historicoRepository = historicoRepository;
        private async Task<bool> PermiteCriarTarefaAsync(int projetoId)
        {
            //var countTarefas = await _tarefaRepository.ContarItemsAsync(filtro: x => x.ProjetoId == projetoId);
            var countTarefas = await _unityOfWork.TarefaRepository.ContarItemsAsync(filtro: x => x.ProjetoId == projetoId);
            return countTarefas < 20;
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> CriarTarefaAsync(
            int projetoId, TarefaDto tarefaDto, string responsavel)
        {
            if (await PermiteCriarTarefaAsync(projetoId))
            {
                var tarefa = new Tarefa
                {
                    Titulo = tarefaDto.Titulo,
                    Descricao = tarefaDto.Descricao,
                    Prioridade = tarefaDto.Prioridade,
                    Status = tarefaDto.Status,
                    Vencimento = tarefaDto.Vencimento,
                    ProjetoId = projetoId
                };

                string evento = "Nova tarefa criada.";

                //await _tarefaRepository.InserirAsync(tarefa);
                await _unityOfWork.TarefaRepository.InserirAsync(tarefa);

                //await _historicoRepository.InserirAsync(
                //    new Historico
                //    {
                //        TarefaId = tarefa.Id,
                //        DataAtualizacao = DateTime.Now,
                //        Responsavel = responsavel,
                //        Resumo = evento
                //    });

                await _unityOfWork.HistoricoRepository.InserirAsync(
                    new Historico
                    {
                        TarefaId = tarefa.Id,
                        DataAtualizacao = DateTime.Now,
                        Responsavel = responsavel,
                        Resumo = evento
                    });

                var dto = new TarefaDto
                {
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Vencimento = tarefa.Vencimento,
                    Status = tarefa.Status,
                    Prioridade = tarefa.Prioridade
                };

                return (true, dto, evento);
            }
            return (false, default!, "O número limite de tarefas foi atingido este projeto.");
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> ExcluirTarefaAsync(
            int tarefaId, string responsavel)
        {
            //var tarefaExcluida = await _tarefaRepository.ExcluirAsync(tarefaId);

            var tarefaExcluida = await _unityOfWork.TarefaRepository.ExcluirAsync(tarefaId);

            if (tarefaExcluida != null)
            {
                string evento = "A tarefa foi removida.";

                //await _historicoRepository.InserirAsync(
                //    new Historico
                //    {
                //        TarefaId = tarefaExcluida.Id,
                //        DataAtualizacao = DateTime.Now,
                //        Responsavel = responsavel,
                //        Resumo = evento
                //    });

                await _unityOfWork.HistoricoRepository.InserirAsync(
                    new Historico
                    {
                        TarefaId = tarefaExcluida.Id,
                        DataAtualizacao = DateTime.Now,
                        Responsavel = responsavel,
                        Resumo = evento
                    });

                await _unityOfWork.SaveChangesAsync();

                var dto = new TarefaDto
                {
                    Titulo = tarefaExcluida.Titulo,
                    Descricao = tarefaExcluida.Descricao,
                    Vencimento = tarefaExcluida.Vencimento,
                    Status = tarefaExcluida.Status,
                    Prioridade = tarefaExcluida.Prioridade
                };

                return (true, dto, evento);
            }

            return (false, default!, "A tarefa não foi encontrada.");
        }
        public async Task<IEnumerable<TarefaDto>?> BuscarTarefasAsync(int projetoId)
        {
            //var tarefaExcluida = await _tarefaRepository.ExcluirAsync(tarefaId);

            var tarefas = await _unityOfWork.TarefaRepository.BuscarVariosAsync(
                t => t.ProjetoId == projetoId);

            var tarefaDtos = tarefas.Select(x =>
            new TarefaDto
            {
                Titulo = x.Titulo,
                Descricao = x.Descricao,
                Vencimento = x.Vencimento,
                Status = x.Status,
                Prioridade = x.Prioridade
            });

            return tarefaDtos;
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto)> AtualizarTarefaAsync(TarefaDto tarefaDto)
        {
            //var tarefaExcluida = await _tarefaRepository.ExcluirAsync(tarefaId);

            var tarefa = await _unityOfWork.TarefaRepository.BuscarUnicoAsync(tarefaDto.Id);

            if (tarefa != null)
            {
                tarefa.Descricao = tarefa.Descricao;
                tarefa.Status = tarefa.Status;
                tarefa.Titulo = tarefa.Titulo;
                // tarefa.Prioridade = tarefa.Prioridade;
                tarefa.Vencimento = tarefa.Vencimento;

                await _unityOfWork.TarefaRepository.EditarAsync(tarefa);

                var dto = new TarefaDto
                {
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Vencimento = tarefa.Vencimento,
                    Status = tarefa.Status,
                    Prioridade = tarefa.Prioridade
                };

                return (true, dto);
            }

            return (false, null!);
        }
    }
}

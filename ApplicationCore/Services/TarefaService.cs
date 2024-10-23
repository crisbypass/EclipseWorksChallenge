using Application.Dtos;
using Application.Interfaces;
using Application.Messages;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class TarefaService(IUnityOfWork unityOfWork)
    {
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        private async Task<bool> PermiteCriarTarefaAsync(int projetoId)
        {
            var countTarefas = await _unityOfWork.TarefaRepository.ContarItemsAsync(
                filtro: x => x.ProjetoId == projetoId);
            return countTarefas < 20;
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> CriarTarefaAsync(
            int projetoId, InputTarefaDto inputTarefaDto)
        {
            if (await PermiteCriarTarefaAsync(projetoId))
            {
                var tarefa = new Tarefa
                {
                    Titulo = inputTarefaDto.Titulo,
                    Descricao = inputTarefaDto.Descricao,
                    Prioridade = inputTarefaDto.Prioridade,
                    Status = inputTarefaDto.Status,
                    Vencimento = inputTarefaDto.Vencimento,
                    ProjetoId = projetoId
                };

                string evento = string.Format(Mensagens.TarefaCriada, tarefa.Id);

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
                        Responsavel = inputTarefaDto.NomeUsuario,
                        Resumo = evento
                    });

                var dto = tarefa.ToTarefaDto();

                return (true, dto, evento);
            }
            return (false, default!, Mensagens.LimiteTarefasAtingido);
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> ExcluirTarefaAsync(
            int tarefaId, string responsavel)
        {
            //var tarefaExcluida = await _tarefaRepository.ExcluirAsync(tarefaId);

            var tarefa = await _unityOfWork.TarefaRepository.ExcluirAsync(tarefaId);

            if (tarefa != null)
            {
                string evento = string.Format(Mensagens.TarefaRemovida, tarefa.Id);

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
                        TarefaId = tarefa.Id,
                        DataAtualizacao = DateTime.Now,
                        Responsavel = responsavel,
                        Resumo = evento
                    });

                await _unityOfWork.SaveChangesAsync();

                var dto = tarefa.ToTarefaDto();

                return (true, dto, evento);
            }

            return (false, null!, Mensagens.TarefaNaoEcontrada);
        }
        public async Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<TarefaDto> TarefaDtos)> BuscarTarefasAsync(int projetoId, int page)
        {
            var (HasPreviousPage, HasNextPage, Items) = await _unityOfWork.TarefaRepository.BuscarVariosAsync(
                x => x.ProjetoId == projetoId, x => x.OrderBy(p => p.Id), page: page);

            var tarefaDtos = Items.Select(x => x.ToTarefaDto());

            return (HasPreviousPage, HasNextPage, tarefaDtos);
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto)> AtualizarTarefaAsync(InputTarefaDto tarefaDto, string responsavel)
        {
            var tarefa = await _unityOfWork.TarefaRepository.BuscarUnicoAsync(tarefaDto.Id);

            if (tarefa != null)
            {
                // A Prioridade não deve ser alterada após a criação do registro.
                // Na descrição desafio, uma alteração qualquer da Tarefa gera um histórico,
                // ao menos no meu entendimento. Vamos comparar quais valores de itens mudaram
                // e criar um registro de histórico para cada uma dessas mudanças.

                var datetimeNow = DateTime.Now;

                if (tarefa.Descricao != tarefaDto.Descricao)
                {
                    tarefa.Descricao = tarefaDto.Descricao;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.DescricaoTarefaAlterada,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Titulo != tarefaDto.Titulo)
                {
                    tarefa.Titulo = tarefaDto.Titulo;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.TituloTarefaAlterado,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Vencimento != tarefaDto.Vencimento)
                {
                    tarefa.Vencimento = tarefaDto.Vencimento;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.VencimentoTarefaAlterado,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Status != tarefaDto.Status)
                {
                    tarefa.Status = tarefaDto.Status;

                    var resumoAlteracao = string.Format(Mensagens.StatusTarefaAlterado, Enum.GetName(StatusEnum.Concluido), tarefa.Id);

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = resumoAlteracao,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                await _unityOfWork.TarefaRepository.EditarAsync(tarefa);

                await _unityOfWork.SaveChangesAsync();

                var dto = tarefa.ToTarefaDto();

                return (true, dto);
            }

            return (false, null!);
        }
        public async Task<(bool Sucesso, ComentarioDto ComentarioDto, string Mensagem)> ComentarAsync(int tarefaId, InputComentarioDto inputComentarioDto)
        {

            var tarefa = await _unityOfWork.TarefaRepository.BuscarUnicoAsync(tarefaId);

            if (tarefa != null)
            {
                var comentario = new Comentario
                {
                    Descricao = inputComentarioDto.Descricao,
                    TarefaId = tarefa.Id,
                    Responsavel = inputComentarioDto.NomeUsuario
                };

                var comentarioInserido = await _unityOfWork.ComentarioRepository.InserirAsync(comentario);

                await _unityOfWork.HistoricoRepository.InserirAsync(
                    new Historico
                    {
                        TarefaId = tarefa.Id,
                        Resumo = Mensagens.ComentarioTarefa,
                        Responsavel = inputComentarioDto.NomeUsuario,
                        DataAtualizacao = DateTime.Now
                    });

                await _unityOfWork.SaveChangesAsync();

                return (true, comentarioInserido.ToComentarioDto(), resumoAlteracao);
            }

            return (false, null!, Mensagens.TarefaNaoEcontrada);
        }

        public async Task<(bool Sucesso,
            IEnumerable<RelatorioDesempenhoDto> RelatorioDesempenhoDtos)> ListarDesempenhoAsync()
        {
            var resumo = string.Format(Mensagens.StatusTarefaAlterado, Enum.GetName(StatusEnum.Concluido));

            var r = await _unityOfWork.HistoricoRepository.BuscarVariosAsync(
                f => f.Resumo == resumo &&
                f.DataAtualizacao >= DateTime.Now.Date.AddDays(-30)
            );

            return default!;
        }

    }
    /// <summary>
    /// Conversão manual simples da entidade Tarefa, para TarefaDto.
    /// </summary>
    /// <remarks>
    /// Para efeitos de simplicidade e performance, esta opção foi escolhida em detrimento
    /// de alternativas como a biblioteca AutoMapper.
    /// </remarks>
    public static class TarefaExtensions
    {
        public static TarefaDto ToTarefaDto(this Tarefa tarefa) =>
            new()
            {
                Id = tarefa.Id,
                ProjetoId = tarefa.ProjetoId,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Prioridade = tarefa.Prioridade,
                Status = tarefa.Status,
                Vencimento = tarefa.Vencimento
            };
        public static HistoricoDto ToHistoricoDto(this Historico historico) =>
            new()
            {

            };
        public static ComentarioDto ToComentarioDto(this Comentario comentario) =>
           new()
           {

           };
    }
}

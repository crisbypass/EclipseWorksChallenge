using Application.Dtos;
using Application.Interfaces;
using Application.Messages;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class TarefaService(IUnityOfWork unityOfWork) : ITarefaService
    {
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        private async Task<bool> PermiteCriarTarefaAsync(int projetoId)
        {
            var countTarefas = await _unityOfWork.TarefaRepository.ContarItensAsync(
                filtro: x => x.ProjetoId == projetoId);
            return countTarefas < 20;
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> CriarTarefaAsync(
            int projetoId, InputTarefaDto inputTarefaDto)
        {
            var project = await _unityOfWork.ProjetoRepository.BuscarUnicoAsync(projetoId);

            if (project != null)
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

                    var inserted = await _unityOfWork.TarefaRepository.InserirAsync(tarefa);

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = inserted.Id,
                            DataAtualizacao = DateTime.Now,
                            Responsavel = inputTarefaDto.NomeUsuario,
                            Resumo = Mensagens.TarefaCriada
                        });

                    await _unityOfWork.SaveChangesAsync();

                    var dto = inserted.ToTarefaDto();

                    return (true, dto, Mensagens.TarefaCriada);
                }

                return (false, default!, Mensagens.LimiteTarefasAtingido);
            }

            return (false, null!, Mensagens.ProjetoNaoEncontrado);
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto)> ExcluirTarefaAsync(int tarefaId)
        {
            var tarefa = await _unityOfWork.TarefaRepository.ExcluirAsync(tarefaId);

            if (tarefa != null)
            {
                await _unityOfWork.SaveChangesAsync();

                var dto = tarefa.ToTarefaDto();

                return (true, dto);
            }

            return (false, null!);
        }
        public async Task<(bool HasPreviousPage, bool HasNextPage,
            IEnumerable<TarefaDto> TarefaDtos, bool IsNotFound)> BuscarTarefasAsync(int projetoId, int page)
        {
            var count = await _unityOfWork.ProjetoRepository.ContarItensAsync(projetoId);

            if (count == 0)
            {
                return (false, false, null!, true);
            }

            var (HasPreviousPage, HasNextPage, Items) = await _unityOfWork.TarefaRepository.BuscarVariosAsync(
                x => x.ProjetoId == projetoId, x => x.OrderBy(p => p.Id), page: page);

            var tarefaDtos = Items.Select(x => x.ToTarefaDto());

            return (HasPreviousPage, HasNextPage, tarefaDtos, false);
        }
        public async Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> AtualizarTarefaAsync(int tarefaId, EditTarefaDto editTarefaDto)
        {
            var tarefa = await _unityOfWork.TarefaRepository.BuscarUnicoAsync(tarefaId);

            if (tarefa != null)
            {
                // A Prioridade não deve ser alterada após a criação do registro.
                // Na descrição do desafio, uma alteração qualquer da Tarefa gera um histórico,
                // ao menos no meu entendimento. Vamos comparar quais valores de itens mudaram
                // e criar um registro de histórico para cada uma dessas mudanças.

                var datetimeNow = DateTime.Now;

                var responsavel = editTarefaDto.NomeUsuario;

                if (tarefa.Descricao != editTarefaDto.Descricao)
                {
                    tarefa.Descricao = editTarefaDto.Descricao;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.DescricaoTarefaAlterada,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Titulo != editTarefaDto.Titulo)
                {
                    tarefa.Titulo = editTarefaDto.Titulo;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.TituloTarefaAlterado,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Vencimento != editTarefaDto.Vencimento)
                {
                    tarefa.Vencimento = editTarefaDto.Vencimento;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = Mensagens.VencimentoTarefaAlterado,
                            Responsavel = responsavel,
                            DataAtualizacao = datetimeNow
                        });
                }

                if (tarefa.Status != editTarefaDto.Status)
                {
                    tarefa.Status = editTarefaDto.Status;

                    var resumoAlteracao = string.Format(Mensagens.StatusTarefaAlterado, Enum.GetName(tarefa.Status), tarefa.Id);

                    bool concluida = tarefa.Status == StatusEnum.Concluido;

                    await _unityOfWork.HistoricoRepository.InserirAsync(
                        new Historico
                        {
                            TarefaId = tarefa.Id,
                            Resumo = resumoAlteracao,
                            Responsavel = responsavel,
                            TarefaConcluida = concluida,
                            DataAtualizacao = datetimeNow
                        });
                }

                var editado = await _unityOfWork.TarefaRepository.EditarAsync(tarefa);

                await _unityOfWork.SaveChangesAsync();

                var dto = editado.ToTarefaDto();

                return (true, dto, Mensagens.TarefaModificada);
            }

            return (false, null!, Mensagens.TarefaNaoEcontrada);
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

                return (true, comentarioInserido.ToComentarioDto(), Mensagens.ComentarioTarefa);
            }

            return (false, null!, Mensagens.TarefaNaoEcontrada);
        }

        public async Task<(bool Sucesso, IEnumerable<RelatorioDesempenhoDto> RelatorioDesempenhoDtos)>
            ListarDesempenhoAsync()
        {

            var lista = await _unityOfWork.HistoricoRepository.BuscarVariosAsync(
                x => x
                .Where(p => p.TarefaConcluida && p.DataAtualizacao >= DateTime.Now.Date.AddDays(-30))
                .Select(p => new { p.Responsavel, p.DataAtualizacao })
                .GroupBy(p => new { p.Responsavel, p.DataAtualizacao })
                .Select(p => new { p.Key.Responsavel, p.Key.DataAtualizacao, Count = p.Count() })
                .OrderBy(p => p.Responsavel)
            );

            var items = lista.Select(x => new RelatorioDesempenhoDto
            {
                NomeUsuario = x.Responsavel,
                TotalTarefasConcluidas = x.Count
            })!;

            return (true, items);
        }
    }
    /// <summary>
    /// Conversão manual de entidades e dtos.
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
        public static ComentarioDto ToComentarioDto(this Comentario comentario) =>
           new()
           {
               Responsavel = comentario.Responsavel,
               Descricao = comentario.Descricao
           };
    }
}

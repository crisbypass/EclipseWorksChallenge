using Application.Dtos;
using Application.Interfaces;
using Application.Messages;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class ProjetoService(IUnityOfWork unityOfWork) : IProjetoService
    {
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        public async Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<ProjetoDto> ProjetoDtos, bool IsNotFound)>
            ListarProjetosAsync(InputProjetoDto inputProjetoDto, int pagina = 1)
        {
            var count = await _unityOfWork.ProjetoRepository.ContarItensAsync(filtro: x => x.NomeUsuario == inputProjetoDto.NomeUsuario);

            if (count == 0)
            {
                return (false, false, null!, true);
            }

            var (HasPreviousPage, HasNextPage, Items) = await _unityOfWork.ProjetoRepository
                .BuscarVariosAsync(x => x.NomeUsuario == inputProjetoDto.NomeUsuario,
                x => x.OrderBy(x => x.Id),
                page: pagina);

            return (HasPreviousPage, HasNextPage, Items.Select(x => x.ToProjetoDto()), false);
        }
        public async Task<(bool Sucesso, ProjetoDto ProjetoDto, string Mensagem)> CriarProjetoAsync(InputProjetoDto projetoDto)
        {
            Projeto projeto = projetoDto.ToProjeto();

            var inserido = await _unityOfWork.ProjetoRepository.InserirAsync(projeto);

            if (inserido == null)
            {
                return (false, null!, Mensagens.FalhaCriacaoProjeto);
            }

            await _unityOfWork.SaveChangesAsync();

            var dto = inserido.ToProjetoDto();

            return (true, dto, Mensagens.ProjetoCriado);
        }
        public async Task<(bool Sucesso, ProjetoDto ProjetoDto, string Mensagem)> ExcluirProjetoAsync(int projetoId)
        {
            var project = await _unityOfWork.ProjetoRepository.BuscarUnicoAsync(projetoId);

            if (project == null)
            {
                return (false, null!, Mensagens.ProjetoNaoEncontrado);
            }

            var count = await _unityOfWork.TarefaRepository.ContarItensAsync(filtro:
                x => x.ProjetoId == project!.Id && x.Status != StatusEnum.Concluido);

            if (project != null && count > 0)
            {
                return (false, null!, Mensagens.ErroExclusaoProjeto);
            }

            var excluido = await _unityOfWork.ProjetoRepository.ExcluirAsync(projetoId);

            await _unityOfWork.SaveChangesAsync();

            var projetoDto = excluido.ToProjetoDto();

            return (true, projetoDto, Mensagens.ProjetoRemovido);
        }
    }
    /// <summary>
    /// Conversão manual de entidades e dtos.
    /// </summary>
    /// <remarks>
    /// Para efeitos de simplicidade e performance, esta opção foi escolhida em detrimento
    /// de alternativas como a biblioteca AutoMapper.
    /// </remarks>
    public static class ProjetoExtensions
    {
        public static ProjetoDto ToProjetoDto(this Projeto projeto) =>
            new()
            {
                Id = projeto.Id,
                NomeUsuario = projeto.NomeUsuario
            };
        public static Projeto ToProjeto(this InputProjetoDto projetoDto) =>
            new()
            {
                NomeUsuario = projetoDto.NomeUsuario
            };
    }
}

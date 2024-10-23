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
        public async Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<ProjetoDto> ProjetoDtos)>
            ListarProjetosAsync(InputProjetoDto inputProjetoDto, int pagina = 1)
        {
            var (HasPreviousPage, HasNextPage, Items) = await _unityOfWork.ProjetoRepository
                .BuscarVariosAsync(x => x.NomeUsuario == inputProjetoDto.NomeUsuario,
                x => x.OrderBy(x => x.Id),
                page: pagina);

            return (HasPreviousPage, HasNextPage, Items.Select(x => x.ToProjetoDto()));
        }
        public async Task<ProjetoDto> CriarProjetoAsync(InputProjetoDto projetoDto)
        {
            Projeto projeto = new()
            {
                NomeUsuario = projetoDto.NomeUsuario
            };

            //var inserido = await _projetoRepository.InserirAsync(projeto);

            var inserido = await _unityOfWork.ProjetoRepository.InserirAsync(projeto);

            var dto = projeto.ToProjetoDto();

            await _unityOfWork.SaveChangesAsync();

            return dto;
        }
        public async Task<(bool Sucesso, ProjetoDto? ProjetoDto, string? Mensagem)> ExcluirProjetoAsync(int projetoId)
        {
            //var project = await _projetoRepository.BuscarUnicoAsync(projetoId,
            //    propriedadesIncluidas: [x =>
            //    x.Tarefas
            //    .Where(x => x.Status != Status.Concluida)
            //    .AsQueryable()]
            //    );

            var project = await _unityOfWork.ProjetoRepository.BuscarUnicoAsync(projetoId,
                propriedadesIncluidas: [x =>
                x.Tarefas
                .Where(x => x.Status != StatusEnum.Concluido)
                .AsQueryable()]
                );

            if (project != null && project.Tarefas.Count > 0)
            {
                return (false, null, Mensagens.ErroExclusaoProjeto);
            }
            else if (project == null)
            {
                return (false, null, null);
            }

            var projetoDto = project.ToProjetoDto();

            return (true, projetoDto, null);
        }
    }
    /// <summary>
    /// Conversão manual simples da entidade Projeto, para ProjetoDto.
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
                NomeUsuario = projeto.NomeUsuario
            };
    }
}

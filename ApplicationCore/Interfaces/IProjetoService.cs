using Application.Dtos;

namespace Application.Services
{
    public interface IProjetoService
    {
        Task<ProjetoDto> CriarProjetoAsync(InputProjetoDto projetoDto);
        Task<(bool Sucesso, ProjetoDto? ProjetoDto, string? Mensagem)> ExcluirProjetoAsync(int projetoId);
        Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<ProjetoDto> ProjetoDtos)> ListarProjetosAsync(InputProjetoDto inputProjetoDto, int pagina = 1);
    }
}
using Application.Dtos;

namespace Application.Services
{
    public interface IProjetoService
    {
        Task<(bool Sucesso, ProjetoDto ProjetoDto, string Mensagem)> CriarProjetoAsync(InputProjetoDto projetoDto);
        Task<(bool Sucesso, ProjetoDto ProjetoDto, string Mensagem)> ExcluirProjetoAsync(int projetoId);
        Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<ProjetoDto> ProjetoDtos, bool IsNotFound)> ListarProjetosAsync(InputProjetoDto inputProjetoDto, int pagina = 1);
    }
}
using Application.Dtos;

namespace Application.Interfaces
{
    public interface ITarefaService
    {
        Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> AtualizarTarefaAsync(int tarefaId, EditTarefaDto tarefaDto);
        Task<(bool HasPreviousPage, bool HasNextPage, IEnumerable<TarefaDto> TarefaDtos, bool IsNotFound)> BuscarTarefasAsync(int projetoId, int page);
        Task<(bool Sucesso, ComentarioDto ComentarioDto, string Mensagem)> ComentarAsync(int tarefaId, InputComentarioDto inputComentarioDto);
        Task<(bool Sucesso, TarefaDto TarefaDto, string Mensagem)> CriarTarefaAsync(int projetoId, InputTarefaDto inputTarefaDto);
        Task<(bool Sucesso, TarefaDto TarefaDto)> ExcluirTarefaAsync(int tarefaId);
        Task<(bool Sucesso, IEnumerable<RelatorioDesempenhoDto> RelatorioDesempenhoDtos)> ListarDesempenhoAsync();
    }
}
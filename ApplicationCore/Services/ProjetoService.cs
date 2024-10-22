using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class ProjetoService(IUnityOfWork unityOfWork)
    {
        private readonly IUnityOfWork _unityOfWork = unityOfWork;

        //private readonly IGenericRepository<Projeto> _projetoRepository = unityOfWork;

        public async Task<ProjetoDto> CriarProjetoAsync(ProjetoDto projetoDto)
        {
            Projeto projeto = new()
            {
                NomeUsuario = projetoDto.NomeUsuario
            };

            //var inserido = await _projetoRepository.InserirAsync(projeto);

            var inserido = await _unityOfWork.ProjetoRepository.InserirAsync(projeto);

            var dto = new ProjetoDto() { };

            await _unityOfWork.SaveChangesAsync();

            return dto;
        }
        public async Task<(bool EmExecucao, ProjetoDto? ProjetoDto)> ExcluirProjetoAsync(int projetoId)
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
                .Where(x => x.Status != Status.Concluida)
                .AsQueryable()]
                );

            if (project != null && project.Tarefas.Count != 0)
            {
                return (true, null);
            }

            return (false, new ProjetoDto { });
        }
    }
}

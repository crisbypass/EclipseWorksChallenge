using EclipseWorksChallenge.MyData.MyEntities;
using EclipseWorksChallenge.MyData.MyRepositories;
using EclipseWorksChallenge.MyDtos;
using EclipseWorksChallenge.MyModels;

namespace EclipseWorksChallenge.MyServices
{
    public class ProjetoService(IGenericRepository<Projeto> projetoRepository)
    {
        private readonly IGenericRepository<Projeto> _projetoRepository = projetoRepository;
        
        public async Task<ProjetoDto> CriarProjetoAsync(ProjetoModel projetoModel)
        {
            Projeto projeto = new()
            {
                NomeUsuario = projetoModel.NomeUsuario
            };

            var inserido = await _projetoRepository.InserirAsync(projeto);

            var dto = new ProjetoDto() { };

            return dto;
        }
        public async Task<(bool EmExecucao, ProjetoDto? ProjetoDto)> ExcluirProjetoAsync(int projetoId)
        {
            var project = await _projetoRepository.BuscarUnicoAsync(projetoId,
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

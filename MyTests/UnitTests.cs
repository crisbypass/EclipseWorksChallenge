using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure.Data.Persistence;
using Infrastructure.Data.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MyTests
{
    public class TestDbContextFixture : MyDbContext
    {
        public TestDbContextFixture() :
            base(new DbContextOptionsBuilder<TestDbContextFixture>().Options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddDebug();
            });

            var logger = loggerFactory.CreateLogger<Program>();

            optionsBuilder.UseInMemoryDatabase("MyInMemoryDatabase")
            .LogTo(message => logger.Log(LogLevel.Information, message));

            base.OnConfiguring(optionsBuilder);
        }
    }
    public class UnitTests(TestDbContextFixture dbFixture) : IClassFixture<TestDbContextFixture>
    {
        private readonly TestDbContextFixture _dbFixture = dbFixture;
        public static TheoryData<InputProjetoDto> InputProjetoDtos => new()
        {
            new InputProjetoDto { NomeUsuario = "User1" },
            new InputProjetoDto { NomeUsuario = "User2" },
            new InputProjetoDto { NomeUsuario = "User3" }
        };

        [Theory]
        [MemberData(nameof(InputProjetoDtos), MemberType = typeof(UnitTests))]
        public async Task CriarProjetoTesteAsync(InputProjetoDto inputProjetoDto)
        {
            IGenericRepository<Projeto> projetoRepository = new GenericRepository<Projeto>(_dbFixture);
            IGenericRepository<Tarefa> tarefaRepository = new GenericRepository<Tarefa>(_dbFixture);
            IGenericRepository<Comentario> comentarioRepository = new GenericRepository<Comentario>(_dbFixture);
            IGenericRepository<Historico> historicoRespository = new GenericRepository<Historico>(_dbFixture);
            IUnityOfWork unityOfWork = new UnityOfWork(_dbFixture, projetoRepository,
                tarefaRepository, historicoRespository, comentarioRepository);

            IProjetoService projetoService = new ProjetoService(unityOfWork);

            var (Sucesso, projectDto, _) = await projetoService
                .CriarProjetoAsync(inputProjetoDto);

            Assert.True(Sucesso);
            Assert.NotNull(projectDto);
        }

        [Theory]
        [MemberData(nameof(InputProjetoDtos), MemberType = typeof(UnitTests))]
        public async Task ListarProjetoTesteAsync(InputProjetoDto inputProjetoDto)
        {
            IGenericRepository<Projeto> projetoRepository = new GenericRepository<Projeto>(_dbFixture);
            IGenericRepository<Tarefa> tarefaRepository = new GenericRepository<Tarefa>(_dbFixture);
            IGenericRepository<Comentario> comentarioRepository = new GenericRepository<Comentario>(_dbFixture);
            IGenericRepository<Historico> historicoRespository = new GenericRepository<Historico>(_dbFixture);
            IUnityOfWork unityOfWork = new UnityOfWork(_dbFixture, projetoRepository,
                tarefaRepository, historicoRespository, comentarioRepository);

            IProjetoService projetoService = new ProjetoService(unityOfWork);

            await projetoService.CriarProjetoAsync(inputProjetoDto);

            var (HasPrevious, HasNext, projetoDtos, NotFound) = await projetoService
                .ListarProjetosAsync(inputProjetoDto);

            Assert.NotNull(projetoDtos);
        }
        public static TheoryData<(InputProjetoDto, InputTarefaDto)> InputTarefaDtos => new()
        {
            (new InputProjetoDto { NomeUsuario = "User1" },
             new InputTarefaDto { NomeUsuario = "User1", Descricao ="Descricao1",
                 Prioridade = Domain.Enums.PrioridadeEnum.Alta, Status = Domain.Enums.StatusEnum.Pendente,
                 Titulo = "Titulo1", Vencimento= DateTime.Now.Date.AddDays(1) }),
            (new InputProjetoDto { NomeUsuario = "User2" },
             new InputTarefaDto { NomeUsuario = "User2", Descricao ="Descricao2",
                 Prioridade = Domain.Enums.PrioridadeEnum.Alta, Status = Domain.Enums.StatusEnum.Pendente,
                 Titulo = "Titulo2", Vencimento= DateTime.Now.Date.AddDays(2) }),
            (new InputProjetoDto { NomeUsuario = "User3" },
             new InputTarefaDto { NomeUsuario = "User3", Descricao ="Descricao3",
                 Prioridade = Domain.Enums.PrioridadeEnum.Alta, Status = Domain.Enums.StatusEnum.Pendente,
                 Titulo = "Titulo3", Vencimento= DateTime.Now.Date.AddDays(3) })
        };

        [Theory]
        [MemberData(nameof(InputTarefaDtos), MemberType = typeof(UnitTests))]
        public async Task CriarTarefaTesteAsync((InputProjetoDto inputProjetoDto, InputTarefaDto inputTarefaDto) values)
        {
            IGenericRepository<Projeto> projetoRepository = new GenericRepository<Projeto>(_dbFixture);
            IGenericRepository<Tarefa> tarefaRepository = new GenericRepository<Tarefa>(_dbFixture);
            IGenericRepository<Comentario> comentarioRepository = new GenericRepository<Comentario>(_dbFixture);
            IGenericRepository<Historico> historicoRespository = new GenericRepository<Historico>(_dbFixture);
            IUnityOfWork unityOfWork = new UnityOfWork(_dbFixture, projetoRepository,
                tarefaRepository, historicoRespository, comentarioRepository);

            IProjetoService projetoService = new ProjetoService(unityOfWork);

            var (inputProjetoDto, inputTarefaDto) = values;

            var (_, inserted, _) = await projetoService.CriarProjetoAsync(inputProjetoDto);

            ITarefaService tarefaService = new TarefaService(unityOfWork);

            (bool Sucesso, TarefaDto tarefaDto, string _) =
                await tarefaService.CriarTarefaAsync(inserted.Id, inputTarefaDto);

            Assert.True(Sucesso);
            Assert.NotNull(tarefaDto);
        }

        [Theory]
        [MemberData(nameof(InputTarefaDtos), MemberType = typeof(UnitTests))]
        public async Task ListarTarefaTesteAsync((InputProjetoDto inputProjetoDto, InputTarefaDto inputTarefaDto) values)
        {
            IGenericRepository<Projeto> projetoRepository = new GenericRepository<Projeto>(_dbFixture);
            IGenericRepository<Tarefa> tarefaRepository = new GenericRepository<Tarefa>(_dbFixture);
            IGenericRepository<Comentario> comentarioRepository = new GenericRepository<Comentario>(_dbFixture);
            IGenericRepository<Historico> historicoRespository = new GenericRepository<Historico>(_dbFixture);
            IUnityOfWork unityOfWork = new UnityOfWork(_dbFixture, projetoRepository,
                tarefaRepository, historicoRespository, comentarioRepository);

            IProjetoService projetoService = new ProjetoService(unityOfWork);

            var (inputProjetoDto, inputTarefaDto) = values;

            var (_, inserted, _) = await projetoService.CriarProjetoAsync(inputProjetoDto);

            ITarefaService tarefaService = new TarefaService(unityOfWork);

            (bool HasPrevious, bool HasNextPage, IEnumerable<TarefaDto> tarefaDtos, bool NotFound) =
                await tarefaService.BuscarTarefasAsync(inserted.Id, 1);

            Assert.NotNull(tarefaDtos);
        }
    }
}
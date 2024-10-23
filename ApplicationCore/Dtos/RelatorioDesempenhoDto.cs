namespace Application.Dtos
{
    public class RelatorioDesempenhoDto
    {
        public string NomeUsuario { get; set; } = default!;
        public int TotalTarefasConcluidas { get; set; }
        public DateTime DataSolicitacao { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsageDashboard.Api.Domain;

[Table("controletributos")]
public sealed class ControleTributo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("codigocontabil")]
    [StringLength(255)]
    public string? CodigoContabil { get; set; }

    [Column("DescricaoCodigoContabil")]
    public string? DescricaoCodigoContabil { get; set; }

    [Column("MesCompetencia")]
    public int? MesCompetencia { get; set; }

    [Column("AnoCompetencia")]
    public int? AnoCompetencia { get; set; }

    [Column("ValorPRovisaoRecolher", TypeName = "decimal(18,2)")]
    public decimal? ValorProvisaoRecolher { get; set; }

    [Column("TipoLancamentoPrevisao")]
    [StringLength(50)]
    public string? TipoLancamentoPrevisao { get; set; }

    [Column("Historico")]
    public string? Historico { get; set; }

    [Column("Vencimento")]
    public DateTime? Vencimento { get; set; }

    [Column("DataRecolhimento")]
    public DateTime? DataRecolhimento { get; set; }

    [Column("ValorRecolhimento", TypeName = "decimal(18,2)")]
    public decimal? ValorRecolhimento { get; set; }

    [Column("TipoLancamento")]
    [StringLength(50)]
    public string? TipoLancamento { get; set; }

    [Column("Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("DataCadastro")]
    public DateTime? DataCadastro { get; set; }

    [Column("DataAlteracao")]
    public DateTime? DataAlteracao { get; set; }

    [Column("Ativo")]
    public bool? Ativo { get; set; }

    [Column("EsteiraID")]
    public int? EsteiraId { get; set; }

    [Column("MensagemID")]
    public int? MensagemId { get; set; }

    [Column("ChaveVinculo")]
    [StringLength(50)]
    public string? ChaveVinculo { get; set; }

    [Column("Origem")]
    public bool? Origem { get; set; }

    [Column("DataLancamento")]
    public DateTime? DataLancamento { get; set; }

    [Column("Regra")]
    [StringLength(150)]
    public string? Regra { get; set; }

    [Column("TipoRegra")]
    [StringLength(250)]
    public string? TipoRegra { get; set; }

    [Column("SaldoInicialConta", TypeName = "decimal(18,2)")]
    public decimal? SaldoInicialConta { get; set; }

    [Column("SaldoFinalConta", TypeName = "decimal(18,2)")]
    public decimal? SaldoFinalConta { get; set; }

    [Column("Lancamento")]
    [StringLength(250)]
    public string? Lancamento { get; set; }

    [Column("Creditos", TypeName = "decimal(18,2)")]
    public decimal? Creditos { get; set; }

    [Column("Debitos", TypeName = "decimal(18,2)")]
    public decimal? Debitos { get; set; }

    [Column("ValorPagamento", TypeName = "decimal(18,2)")]
    public decimal? ValorPagamento { get; set; }

    [Column("ValorDesconto", TypeName = "decimal(18,2)")]
    public decimal? ValorDesconto { get; set; }

    [Column("SaldoFinalReal", TypeName = "decimal(18,2)")]
    public decimal? SaldoFinalReal { get; set; }

    [Column("LancamentoInicial")]
    [StringLength(250)]
    public string? LancamentoInicial { get; set; }

    [Column("ChaveVinculoMatch")]
    [StringLength(255)]
    public string? ChaveVinculoMatch { get; set; }

    [Column("NovoHistorico")]
    public string? NovoHistorico { get; set; }

    [Column("Manteve")]
    public bool? Manteve { get; set; } = false;

    [Column("HistoricoOriginal")]
    public string? HistoricoOriginal { get; set; }

    [Column("CodigoContabilOriginal")]
    [StringLength(255)]
    public string? CodigoContabilOriginal { get; set; }

    [Column("ValorAnterior", TypeName = "decimal(18,2)")]
    public decimal? ValorAnterior { get; set; }

    [Column("ContaContraPartida")]
    [StringLength(100)]
    public string? ContaContraPartida { get; set; }

    [Column("usuarioConciliou")]
    public int? UsuarioConciliou { get; set; }

    [Column("pontos")]
    public int? Pontos { get; set; } = 0;

    [Column("lancamentosRelacionados")]
    [StringLength(255)]
    public string? LancamentosRelacionados { get; set; }

    [Column("arquivo_id")]
    public int? ArquivoId { get; set; }

    [Column("isSaldoInicial")]
    public bool? IsSaldoInicial { get; set; } = false;

    [Column("empresaId")]
    public int? EmpresaId { get; set; }

    [Column("relatorioId")]
    public int? RelatorioId { get; set; }
}

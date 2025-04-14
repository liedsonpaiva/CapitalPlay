using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Gerencia as ações do mercado, aplicando flutuações de preço com base em eventos.
/// </summary>
public partial class Mercado : Node
{
    private List<Acoes> acoes = new List<Acoes>();

    public float Preco { get; set; } // antes estava apenas com get;

    public void Subida(Acoes acao)
    {
        acao.Preco += RoladorDeDados.D6();
        acao.Preco = Math.Max(acao.Preco, 0.01f);
    }

    public void Queda(Acoes acao)
    {
        acao.Preco -= RoladorDeDados.D6();
        acao.Preco = Math.Max(acao.Preco, 0.01f);
    }

    public void Volatil(Acoes acao)
    {
        // Varia mais drasticamente (com D66)
        acao.Preco += RoladorDeDados.D66();
        acao.Preco = Math.Max(acao.Preco, 0.01f);
    }

    private void AdicionarAcao(string nicho, float preco)
    {
        acoes.Add(new Acoes(nicho, preco));
    }

    public void InicializarAcoes()
    {
        AdicionarAcao("Transporte", RoladorDeDados.D8());
        AdicionarAcao("Siderúrgica", RoladorDeDados.D8());
        AdicionarAcao("Tecnologia", RoladorDeDados.D8());
        AdicionarAcao("Saúde", RoladorDeDados.D8());
        AdicionarAcao("Alimentação", RoladorDeDados.D8());
    }

    public void MudarValorAcoes(CartaInformacao carta)
    {
        var acao1 = BuscarAcaoPorNicho(carta.Nicho);
        if (acao1 == null)
        {
            GD.PrintErr($"Erro: Ação não encontrada para nicho '{carta.Nicho}'");
            return;
        }

        // Impacto direto da carta
        if (carta.Informacao)
        {
            GD.Print($"Subida para o nicho '{acao1.Nicho}'");
            Subida(acao1);
        }
        else
        {
            GD.Print($"Queda para o nicho '{acao1.Nicho}'");
            Queda(acao1);
        }

        GD.Print($"Preço após impacto direto: R$ {acao1.Preco:0.00}");

        // Volatilidade para os outros setores
        foreach (var acao in acoes)
        {
            if (acao.Nicho != acao1.Nicho)
            {
                GD.Print($"Volatilidade para o nicho '{acao.Nicho}'");
                Volatil(acao);
                GD.Print($"Novo preço do nicho '{acao.Nicho}': R$ {acao.Preco:0.00}");
            }
        }
    }

    public Acoes BuscarAcaoPorNicho(string nicho)
    {
        foreach (var acao in acoes)
        {
            if (acao.Nicho == nicho)
                return acao;
        }
        return null;
    }

    public List<Acoes> GetTodasAcoes()
    {
        // Retorna uma cópia para proteger a lista original
        return new List<Acoes>(acoes);
    }
}
